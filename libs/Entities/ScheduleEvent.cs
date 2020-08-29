using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ScheduleEvent class, provides the ORM a way to manage the many-to-many relationship between Schedule and Event.
    /// </summary>
    public class ScheduleEvent
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the schedule.
        /// </summary>
        public int ScheduleId { get; set; }

        /// <summary>
        /// get/set - The schedule.
        /// </summary>
        public Schedule Schedule { get; set; }

        /// <summary>
        /// get/set - Foreign key to the event.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// get/set - The event.
        /// </summary>
        public Event Event { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ScheduleEvent object.
        /// </summary>
        public ScheduleEvent()
        { }

        /// <summary>
        /// Creates a new instance of a ScheduleEvent object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="cevent"></param>
        public ScheduleEvent(Schedule schedule, Event cevent)
        {
            this.ScheduleId = schedule?.Id ?? throw new ArgumentNullException(nameof(schedule));
            this.Schedule = schedule;
            this.EventId = cevent?.Id ?? throw new ArgumentNullException(nameof(cevent));
            this.Event = cevent;
        }
        #endregion
    }
}
