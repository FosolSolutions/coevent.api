using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoEvent.Api.Areas.Data.Controllers
{
    /// <summary>
    /// OpeningController sealed class, provides API endpoints for calendar event openings.
    /// </summary>
    [Produces("application/json")]
    [Area("data")]
    [Route("[area]/calendar/event/activity/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class OpeningController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningController object.
        /// </summary>
        /// <param name="datasource"></param>
        public OpeningController(IDataSource datasource)
        {
            _dataSource = datasource;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an event opening for the specified 'id'.
        /// </summary>
        /// <param name="id">The primary key for the opening.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetOpening(int id)
        {
            var opening = _dataSource.Openings.Get(id);
            return Ok(opening);
        }

        /// <summary>
        /// Returns an array of openings for the calendar specified by the 'id'.
        /// </summary>
        /// <param name="id">The calendar id.</param>
        /// <param name="startOn">The start date for the calendar to return.  Defaults to now.</param>
        /// <param name="endOn">The end date for the calendar to return.</param>
        /// <returns>An array of events.</returns>
        [HttpGet("/[area]/calendar/{id}/openings")]
        public IActionResult GetOpeningsForCalendar(int id, DateTime? startOn = null, DateTime? endOn = null)
        {
            var start = startOn ?? DateTime.UtcNow;
            // Start at the beginning of the week.
            start = start.DayOfWeek == DayOfWeek.Sunday ? start : start.AddDays(-1 * (int)start.DayOfWeek);
            var end = endOn ?? start.AddDays(7);

            var openings = _dataSource.Openings.GetForCalendar(id, start, end);
            return Ok(openings);
        }

        /// <summary>
        /// Submit an application for the current participant.
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        [HttpPut("apply")]
        public IActionResult Apply([FromBody] CoEvent.Models.OpeningApplication application)
        {
            var result = _dataSource.Openings.Apply(application);
            _dataSource.CommitTransaction();

            return Ok(result);
        }

        /// <summary>
        /// Remove an application for the current participant.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut("unapply")]
        public IActionResult Unapply([FromBody] CoEvent.Models.Opening opening)
        {
            var result = _dataSource.Openings.Unapply(opening);
            _dataSource.CommitTransaction();

            return Ok(result);
        }
        #endregion
    }
}
