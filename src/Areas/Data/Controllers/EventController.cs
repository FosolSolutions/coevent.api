using CoEvent.Core.Extensions;
using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CoEvent.Api.Areas.Data.Controllers
{
    /// <summary>
    /// EventController sealed class, provides API endpoints for calendar events.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class EventController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventController object.
        /// </summary>
        /// <param name="datasource"></param>
        public EventController(IDataSource datasource)
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
        [HttpGet("{id}")]
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
        [HttpGet("/[area]/calendar/{id}/events")]
        public IActionResult GetEventsForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            var cevents = _dataSource.Events.GetForCalendar(id, start, end);
            return Ok(cevents);
        }

        /// <summary>
        /// Returns an array of events for the specified event 'ids'.
        /// This will include all activities and openings.
        /// Only returns events for the currently selected calendar.
        /// </summary>
        /// <param name="ids">A comma-separated list of event 'id' values (i.e. ids=1,2,3,4).</param>
        /// <returns></returns>
        [HttpGet("/[area]/calendar/events")]
        public IActionResult GetEvents([FromQuery] string ids)
        {
            var values = ids?.Split(',').Select(v => v.Trim().ConvertTo<int>()).Distinct().ToArray();
            var cevents = _dataSource.Events.Get(values);
            return Ok(cevents);
        }
        #endregion
    }
}
