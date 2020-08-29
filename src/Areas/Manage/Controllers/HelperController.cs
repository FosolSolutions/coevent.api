using CoEvent.Core.Mvc;
using CoEvent.Api.Helpers.Mail;
using CoEvent.Data.Interfaces;
using CoEvent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoEvent.Api.Areas.Manage.Controllers
{
    /// <summary>
    /// CalendarController class, provides API endpoints for calendars.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/[controller]")]
    [Authorize]
    public sealed class HelperController : ApiController
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
        public HelperController(IDataSource dataSource, MailClient mailClient)
        {
            _dataSource = dataSource;
            _mailClient = mailClient;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and adds all events for the specified ecclesia.
        /// </summary>
        /// <param name="calendar">The calendar the will be updated in the datasource. JSON data object in the body of the request.</param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns>The calendar that was updated and all the newly added events in the datasource.</returns>
        [HttpPost("ecclesia")]
        public IActionResult AddEcclesialEvents([FromBody] Calendar calendar, [FromQuery] DateTime? startOn, [FromQuery] DateTime? endOn)
        {
            var result = _dataSource.Helper.AddEcclesialEvents(calendar.Id.Value, startOn, endOn);

            return Ok(result);
        }

        /// <summary>
        /// Adds all the members of the CoEventtoria ecclesia to the specified calendar.
        /// </summary>
        /// <param name="calendar">The calendar the will be updated in the datasource. JSON data object in the body of the request.</param>
        /// <returns>An array of participants that were added.</returns>
        [HttpPost("ecclesia/participants")]
        public IActionResult AddEcclesialParticipants([FromBody] Calendar calendar)
        {
            var result = _dataSource.Helper.AddParticipants(calendar.Id.Value);

            return Ok(result);
        }
        #endregion
    }
}
