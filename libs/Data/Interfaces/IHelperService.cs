using System;
using System.Collections.Generic;

namespace CoEvent.Data.Interfaces
{
    public interface IHelperService
    {
        #region Methods
        /// <summary>
        /// Creates and adds a new ecclesial calendar with default events, activities, openings and criteria.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        Models.Calendar AddEcclesialEvents(int calendarId, DateTime? startOn = null, DateTime? endOn = null);


        /// <summary>
        /// Adds a predefined list of participants to the specified calendar.
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        IEnumerable<Models.Participant> AddParticipants(int calendarId);
        #endregion
    }
}
