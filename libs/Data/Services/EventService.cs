using CoEvent.Core.Exceptions;
using CoEvent.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// EventService sealed class, provides a way to manage events in the datasource.
    /// </summary>
    public sealed class EventService : UpdatableService<Entities.Event, Models.Event>, IEventService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EventService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal EventService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the event for the specified 'id'.
        /// Validates whether the current user is authorized to view the event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Event Get(int id)
        {
            // TODO: Is user allowed ot see event?
            return this.Map(this.Find((set) => set.Include(c => c.Criteria).Include(c => c.Tags).Include(e => e.Activities).SingleOrDefault(e => e.Id == id)));
        }

        /// <summary>
        /// Get the events for the specified calendar, within the specified date range.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Event> GetForCalendar(int calendarId, DateTime startOn, DateTime endOn)
        {
            var participantId = this.GetParticipantId();
            var isAuthorized = this.Context.Calendars.Any(c => c.Id == calendarId && c.Participants.Any(p => p.Id == participantId));
            if (!isAuthorized) throw new NotAuthorizedException();

            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var events = (
                from e in this.Context.Events
                    .Include(e => e.Criteria)
                    .Include(e => e.Tags)
                where e.CalendarId == calendarId && e.StartOn >= start && e.EndOn <= end
                orderby e.StartOn, e.Name
                select this.Map(e)).ToArray();
            return events;
        }

        /// <summary>
        /// Get the event Ids for the specified schedule.
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public IEnumerable<int> GetEventIdsForSchedule(int scheduleId)
        {
            return this.Context.Schedules.Where(s => s.Id == scheduleId).ToArray().SelectMany(s => s.Events.Select(e => e.EventId)).ToArray();
        }

        /// <summary>
        /// Get all the events, their activies and openings for the specified event ids.
        /// Only returns events for the currently selected calendar.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Models.Event> Get(int[] ids)
        {
            // TODO: configure limit to ids.
            var calendarId = this.GetCalendarId();

            var events = this.Context.Events
                .Include(e => e.Criteria)
                .Include(e => e.Activities).ThenInclude(a => a.Criteria).ThenInclude(c => c.Criteria)
                .Include(e => e.Activities).ThenInclude(a => a.Openings).ThenInclude(o => o.Criteria).ThenInclude(c => c.Criteria)
                .Where(e => e.CalendarId == calendarId && ids.Contains(e.Id));

            return events.ToArray().Select(e => this.Map(e)).ToArray();
        }
        #endregion
    }
}
