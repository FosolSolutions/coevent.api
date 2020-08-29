using CoEvent.Data.Interfaces;
using System;
using System.Linq;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// ScheduleService sealed class, provides a way to manage schedules in the datasource.
    /// </summary>
    public sealed class ScheduleService : UpdatableService<Entities.Schedule, Models.Schedule>, IScheduleService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal ScheduleService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the schedule for the specified 'id'.
        /// Validates whether the current user is authorized to view the schedule.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Schedule Get(int id)
        {
            // TODO: Is user allowed ot see schedule?
            return this.Map(this.Find((set) => set.SingleOrDefault(e => e.Id == id)));
        }

        /// <summary>
        /// Get the schedule for the specified 'id'.
        /// Validates whether the current user is authorized to view the schedule.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Schedule Get(int id, DateTime? startOn, DateTime? endOn)
        {
            var schedule = this.Get(id);

            // TODO: Add events.
            return schedule;
        }
        #endregion
    }
}
