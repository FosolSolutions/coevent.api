using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Filters;
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoEvent.Api.Areas.Manage.Controllers
{
    /// <summary>
    /// OpeningsController sealed class, provides API endpoints for calendar event openings.
    /// </summary>
    [Produces("application/json")]
    [Area("manage")]
    [Route("[area]/calendars/events/activities/[controller]")]
    [Authorize]
    [ValidateModelFilter]
    public sealed class OpeningsController : ApiController
    {
        #region Variables
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningsController object.
        /// </summary>
        /// <param name="datasource"></param>
        public OpeningsController(IDataSource datasource)
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
        [HttpGet("{id}", Name = "GetOpening")]
        public IActionResult GetOpening(int id)
        {
            var opening = _dataSource.Openings.Get(id);
            return Ok(opening);
        }

        /// <summary>
        /// Add the new opening to the datasource.
        /// </summary>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPost()]
        public IActionResult AddOpening([FromBody] CoEvent.Models.Opening opening)
        {
            _dataSource.Openings.Add(opening);
            _dataSource.CommitTransaction();

            return Created(Url.RouteUrl(nameof(GetOpening), new { opening.Id }), opening);
        }

        /// <summary>
        /// Update the specified opening in the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateOpening(int id, [FromBody] CoEvent.Models.Opening opening)
        {
            _dataSource.Openings.Update(opening);
            _dataSource.CommitTransaction();

            return Ok(opening);
        }

        /// <summary>
        /// Delete the specified opening from the datasource.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="opening"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteOpening(int id, [FromBody] CoEvent.Models.Opening opening)
        {
            _dataSource.Openings.Remove(opening);
            _dataSource.CommitTransaction();

            return Ok();
        }
        #endregion
    }
}
