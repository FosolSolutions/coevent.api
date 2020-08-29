using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Event : BaseModel
    {
        #region Properties
        public int? Id { get; set; }

        public int CalendarId { get; set; }

        public Guid? Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public Data.Entities.EventState State { get; set; }

        public Data.Entities.CriteriaRule CriteriaRule { get; set; }

        public int? ParentEventId { get; set; }

        /// <summary>
        /// get/set - How often this event repeats.
        /// </summary>
        public Data.Entities.EventRepetition Repetition { get; set; }

        /// <summary>
        /// get/set - When the repeat will end.
        /// </summary>
        public DateTime? RepetitionEndOn { get; set; }

        /// <summary>
        /// get/set - The size of the delta between each repeated event (i.e. days, weeks, months, etc).  This value is influenced by the 'Repetition' property.
        /// </summary>
        public int RepetitionSize { get; set; }

        public IEnumerable<Activity> Activities { get; set; }

        public IEnumerable<Criteria> Criteria { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        #endregion
    }
}
