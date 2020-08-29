using System;
using System.Collections.Generic;

namespace CoEvent.Data.Interfaces
{
    public interface IScheduleService : IUpdatableService<Models.Schedule>
    {
        /// <summary>
        /// Get the schedule for the specified 'id'.
        /// Validates whether the current user is authorized to view the schedule.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Schedule Get(int id);

        /// <summary>
        /// Get the schedule for the specified 'id'.
        /// Validates whether the current user is authorized to view the schedule.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        /// <returns></returns>
        Models.Schedule Get(int id, DateTime? startOn = null, DateTime? endOn = null);
    }
}
