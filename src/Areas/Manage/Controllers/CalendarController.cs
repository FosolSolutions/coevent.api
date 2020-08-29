using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Api.Helpers;
using CoEvent.Api.Helpers.Mail;
using CoEvent.Data.Interfaces;
using CoEvent.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoEvent.Api.Areas.Manage.Controllers
{
    /// <summary>
    /// CalendarController class, provides API endpoints for calendars.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class CalendarController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        private readonly MailClient _mailClient;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarController object.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="mailClient"></param>
        public CalendarController(IDataSource dataSource, MailClient mailClient)
        {
            _dataSource = dataSource;
            _mailClient = mailClient;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an array of all the calendars for the current user.
        /// </summary>
        /// <param name="page">The page number (default: 1).</param>
        /// <returns>An array calendar JSON data objects.</returns>
        [HttpGet("/[area]/calendars")]
        public IActionResult GetCalendars(int page)
        {
            var skip = page <= 0 ? 0 : page - 1;
            // TODO: Configurable 'take'.
            // TODO: no tracking.
            var calendars = _dataSource.Calendars.Get(skip, 10);

            return calendars.Count() != 0 ? Ok(calendars) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Returns the specified calendar and its events for the current week (or timespan).
        /// </summary>
        /// <param name="id">The primary key to identify the calendar.</param>
        /// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the calendar to return.</param>
        /// <returns>A calendar JSON data object with all events within the specified date range.</returns>
        [HttpGet("{id}", Name = nameof(GetCalendar))]
        public IActionResult GetCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            // TODO: no tracking.
            var calendar = _dataSource.Calendars.Get(id, start, end);
            return calendar != null ? Ok(calendar) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Add the specified calendar to the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpPost("/[area]/[controller]")]
        public IActionResult AddCalendar([FromBody] Calendar calendar)
        {
            _dataSource.Calendars.Add(calendar);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetCalendar), new { calendar.Id }), calendar);
        }

        /// <summary>
        /// Update the specified calendar in the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpPut("/[area]/[controller]")]
        public IActionResult UpdateCalendar([FromBody] Calendar calendar)
        {
            _dataSource.Calendars.Update(calendar);
            _dataSource.CommitTransaction();

            return Ok(calendar);
        }

        /// <summary>
        /// Delete the specified calendar from the datasource.
        /// </summary>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpDelete("/[area]/[controller]")]
        public IActionResult DeleteCalendar([FromBody] Calendar calendar)
        {
            _dataSource.Calendars.Remove(calendar);
            _dataSource.CommitTransaction();

            return Ok();
        }

        /// <summary>
        /// Makes the specified calendar the active calendar for the currently signed in user.
        /// Updates the users claims.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        [HttpPut("select/{calendarId}"), Authorize]
        public async Task<IActionResult> SelectCalendar(int calendarId)
        {
            _dataSource.Calendars.SelectCalendar(User, calendarId);
            var principal = new ClaimsPrincipal(User);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Ok(true);
        }

        /// <summary>
        /// Send emails out to all the participants in the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns>Either 'true' for full success, or a collection of errors that occured when sending emails.</returns>
        [HttpPut("{calendarId}/invite/participants")]
        public IActionResult InviteParticipants(int calendarId)
        {
            var participants = _dataSource.Participants.GetForCalendar(calendarId);
            var errors = new List<Exception>();

            foreach (var participant in participants)
            {
                if (!String.IsNullOrWhiteSpace(participant.Email))
                {
                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(1000);

                        try
                        {
                            var message = _mailClient.CreateInvitation(participant);
                            await _mailClient.SendAsync(message);
                        }
                        catch (Exception ex)
                        {
                            // TODO: Handle and log errors differently.
                            errors.Add(new Exception($"Mail failed for {participant.DisplayName} - {participant.Email}", ex));
                        }
                    });

                    t.Wait();
                }
            }

            if (errors.Count() == 0)
            {
                return new JsonResult(true);
            }

            // TODO: Don't include certain information in error message.
            return new JsonResult(errors.Select(e => new { e.Message, InnerException = e.InnerException.Message }));

        }
        #endregion
    }
}
