using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoEvent.Data.Interfaces
{
    public interface ICalendarService : IUpdatableService<Models.Calendar>
    {
        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// Validates whether the current user is authorized to view the calendar.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Calendar Get(int id);

        /// <summary>
        /// Get all of the calendars for the current user.
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<Models.Calendar> Get(int skip, int take);

        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// Validates whether the current user is authorized to view the calendar.
        /// Includes events for the specified timeframe.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startOn"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Models.Calendar Get(int id, DateTime startOn, DateTime endOn);

        /// <summary>
        /// Get the claims associated with this calendar.
        /// Validates whether the current user  is authorized to view the calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        IEnumerable<Claim> GetClaims(int calendarId);
    }
}
