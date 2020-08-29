using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Schedule class, provides a way to filter a collection of events from multiple calendars.
    /// </summary>
    public class Schedule : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this schedule.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account that own's this schedule.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account that own's this schedule.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// get/set - A name to identify the schedule.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the schedule.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The start date of the schedule.
        /// </summary>
        public DateTime StartOn { get; set; }

        /// <summary>
        /// get/set - The end date of the schedule.
        /// </summary>
        public DateTime EndOn { get; set; }

        /// <summary>
        /// get/set - The current state of the schedule.
        /// </summary>
        public ScheduleState State { get; set; } = ScheduleState.Published;

        /// <summary>
        /// get - A collection of events associated with this schedule.
        /// </summary>
        public ICollection<ScheduleEvent> Events { get; private set; } = new List<ScheduleEvent>(); // TODO: Must use many-to-many table because events are linked to Calendar and not CoEvent.  Or I need to instead create a filter which will pull in events from calendars.
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Schedule object.
        /// </summary>
        public Schedule()
        {

        }

        /// <summary>
        /// Creates a new instance of a Schedule object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="startOn"></param>
        /// <param name="endOn"></param>
        public Schedule(Account account, string name, DateTime startOn, DateTime endOn)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument 'name' cannot be null, empty or whitespace.");
            this.AccountId = account?.Id ?? throw new ArgumentNullException(nameof(account));
            this.Account = account;
            this.Name = name;
            this.Key = Guid.NewGuid();
            this.StartOn = startOn;
            this.EndOn = endOn;
        }
        #endregion
    }
}
