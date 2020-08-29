using CoEvent.Core.Exceptions;
using CoEvent.Data.Interfaces;
using CoEvent.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// ActivityService sealed class, provides a way to manage activities in the datasource.
    /// </summary>
    public sealed class ActivityService : UpdatableService<Activity, Models.Activity>, IActivityService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal ActivityService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the activity for the specified 'id'.
        /// Validates whether the current user is authorized to view the activity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Activity Get(int id)
        {
            var calendarId = this.GetCalendarId();
            return this.Map(this.Find((set) => set
                .Include(c => c.Criteria).ThenInclude(ac => ac.Criteria)
                .Include(c => c.Tags)
                .Include(a => a.Openings)
                .SingleOrDefault(a => a.Id == id && a.Event.CalendarId == calendarId)));
        }

        /// <summary>
        /// Get the activities for the specified 'eventId' and within the specified timeframe.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Activity> GetForEvent(int eventId, DateTime? startOn = null, DateTime? endOn = null)
        {
            var calendarId = this.GetCalendarId();
            var cevent = this.Find<Event>((set) => set.SingleOrDefault(e => e.Id == eventId && e.CalendarId == calendarId));

            // Convert datetime to utc.
            var start = startOn?.ToUniversalTime() ?? cevent.StartOn;
            var end = endOn?.ToUniversalTime() ?? cevent.EndOn;

            var activities = this.Context.Activities
                .Include(a => a.Criteria)
                .Include(a => a.Openings).ThenInclude(o => o.Criteria)
                .Where(a => a.EventId == eventId && a.StartOn >= start && a.EndOn <= end)
                .OrderBy(a => a.StartOn)
                .ThenBy(a => a.Sequence)
                .ToArray()
                .Select(a => this.Map(a));
            return activities;
        }

        /// <summary>
        /// Get the activities for the specified 'calendarId' and within the specified timeframe.
        /// Validates whether the current use is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        public IEnumerable<Models.Activity> GetForCalendar(int calendarId, DateTime startOn, DateTime endOn)
        {
            var participantId = this.GetParticipantId();
            var isAuthorized = this.Context.Calendars.Any(c => c.Id == calendarId && c.Participants.Any(p => p.Id == participantId));
            if (!isAuthorized) throw new NotAuthorizedException();

            // Convert datetime to utc.
            var start = startOn.ToUniversalTime();
            var end = endOn.ToUniversalTime();

            var activities = (
                from a in this.Context.Activities
                    .Include(a => a.Criteria).ThenInclude(c => c.Criteria)
                    .Include(a => a.Tags)
                where a.Event.CalendarId == calendarId
                    && a.StartOn >= startOn
                    && a.EndOn <= endOn
                orderby a.StartOn, a.Sequence
                select a
                ).ToArray()
                .Select(a => this.Map(a));

            return activities;
        }
        #endregion
    }
}
