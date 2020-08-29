using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class Event : BaseReadonlyModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.  Unique way to identify the event.
        /// </summary>
        public int Id { get; set; }

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
        public Data.Entities.EventState State { get; set; } = Data.Entities.EventState.Published;

        /// <summary>
        /// get/set - How criteria is applied to participants in this event.  This can be overridden in child entities.
        /// </summary>
        public Data.Entities.CriteriaRule CriteriaRule { get; set; } = Data.Entities.CriteriaRule.Participate;

        /// <summary>
        /// get/set - Foreign key to the parent event.
        /// </summary>
        public int? ParentEventId { get; set; }

        /// <summary>
        /// get/set - How often this event repeats.
        /// </summary>
        public Data.Entities.EventRepetition Repetition { get; set; } = Data.Entities.EventRepetition.None;

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
        public IList<Criteria> Criteria { get; private set; }

        /// <summary>
        /// get - A collection of activities within this event.
        /// </summary>
        public IList<Activity> Activities { get; private set; }

        /// <summary>
        /// get - A collection of tags associated with this event.
        /// </summary>
        public IList<Tag> Tags { get; private set; }
        #endregion
    }
}
