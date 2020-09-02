using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Data.Interfaces;
using CoEvent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CoEvent.Api.Areas.Manage.Controllers
{
    /// <summary>
    /// EventsController sealed class, provides API endpoints for calendar events.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/calendars/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class EventsController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventsController object.
        /// </summary>
        /// <param name="datasource"></param>
        public EventsController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns a calendar event for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the event.</param>
        /// <returns>An event for the specified 'id'.</returns>
        [HttpGet("{id}", Name = nameof(GetEvent))]
        public IActionResult GetEvent(int id)
        {
            var cevent = _dataSource.Events.Get(id);
            return Ok(cevent);
        }

        /// <summary>
        /// Returns an array of events for the calendar specified by the 'id'.
        /// </summary>
        /// <param name="id">The calendar id.</param>
        /// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the calendar to return.</param>
        /// <returns>An array of events.</returns>
        [HttpGet("/[area]/calendars/{id}/[controller]")]
        public IActionResult GetEventsForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            var cevents = _dataSource.Events.GetForCalendar(id, start, end);
            return cevents.Count() != 0 ? Ok(cevents) : (IActionResult)NoContent();
        }

        /// <summary>
        /// Adds the new event to the datasource.
        /// </summary>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddEvent([FromBody] Event cevent)
        {
            _dataSource.Events.Add(cevent);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetEvent), new { cevent.Id }), cevent);
        }

        /// <summary>
        /// Updates the specified event in the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, [FromBody] Event cevent)
        {
            _dataSource.Events.Update(cevent);
            _dataSource.CommitTransaction();

            return Ok(cevent);
        }

        /// <summary>
        /// Deletes the specified event from the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cevent"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id, [FromBody] Event cevent)
        {
            _dataSource.Events.Remove(cevent);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
