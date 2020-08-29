using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// <typeparamref name="Event"/> class, provides a way to manage events in the datasource.  An event is a specific time slot in a calendar.
    /// </summary>
    public class Event : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.  Unique way to identify the event.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar.  All events are owned by a calendar.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar this event belongs to.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this event.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A name to identify the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - An event always has a start date and time.
        /// </summary>
        public DateTime StartOn { get; set; }

        /// <summary>
        /// get/set - An event always has an end date and time.
        /// </summary>
        public DateTime EndOn { get; set; }

        /// <summary>
        /// get/set - The current state of the event.
        /// </summary>
        public EventState State { get; set; } = EventState.Published;

        /// <summary>
        /// get/set - How criteria is applied to participants in this event.  This can be overridden in child entities.
        /// </summary>
        public CriteriaRule CriteriaRule { get; set; } = CriteriaRule.Participate;

        /// <summary>
        /// get/set - Foreign key to the parent event.
        /// </summary>
        public int? ParentEventId { get; set; }

        /// <summary>
        /// get/set - The event that this event originated from.
        /// </summary>
        public Event ParentEvent { get; set; }

        /// <summary>
        /// get/set - How often this event repeats.
        /// </summary>
        public EventRepetition Repetition { get; set; } = EventRepetition.None;

        /// <summary>
        /// get/set - When the repeat will end.
        /// </summary>
        public DateTime? RepetitionEndOn { get; set; }

        /// <summary>
        /// get/set - The size of the delta between each repeated event (i.e. days, weeks, months, etc).  This value is influenced by the 'Repetition' property.
        /// </summary>
        public int RepetitionSize { get; set; }

        /// <summary>
        /// get - A collection of criteria which are required to participate in the events.
        /// </summary>
        public ICollection<EventCriteria> Criteria { get; private set; } = new List<EventCriteria>();

        /// <summary>
        /// get - A collection of activities within this event.
        /// </summary>
        public ICollection<Activity> Activities { get; private set; } = new List<Activity>();

        /// <summary>
        /// get - A collection of tags associated with this event.
        /// </summary>
        public ICollection<EventTag> Tags { get; private set; } = new List<EventTag>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a <typeparamref name="Event"/> object.
        /// </summary>
        public Event()
        {

        }

        /// <summary>
        /// Creates a new instance of a <typeparamref name="Event"/> object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="state"></param>
        /// <param name="criteriaRule"></param>
        public Event(Calendar calendar, string name, DateTime start, DateTime end, EventState state = EventState.Published, CriteriaRule criteriaRule = CriteriaRule.Participate)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.Name = name;
            this.Key = Guid.NewGuid();
            this.StartOn = start;
            this.EndOn = end;
            this.State = state;
            this.CriteriaRule = CriteriaRule;
        }
        #endregion
    }
}
