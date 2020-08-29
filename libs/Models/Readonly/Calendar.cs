using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class Calendar : BaseReadonlyModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the account that owns this calendar.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this calendar.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name within an account that identifies this calendar.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the calendar.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The state of the calendar.
        /// </summary>
        public Data.Entities.CalendarState State { get; set; } = Data.Entities.CalendarState.Published;

        /// <summary>
        /// get/set - How criteria is applied to participants in this calendar.  This can be overridden in child entities.
        /// </summary>
        public Data.Entities.CriteriaRule CriteriaRule { get; set; } = Data.Entities.CriteriaRule.Participate;

        /// <summary>
        /// get - A collection of events within this calendar.
        /// </summary>
        public IList<Event> Events { get; set; }

        /// <summary>
        /// get - A collection of participants within this calendar.
        /// </summary>
        public IList<Participant> Participants { get; set; }

        /// <summary>
        /// get - A collection of criteria for this calendar.
        /// </summary>
        public IList<Criteria> Criteria { get; set; }

        /// <summary>
        /// get - A collection of tags for this calendar.
        /// </summary>
        public IList<Tag> Tags { get; set; }
        #endregion
    }
}
