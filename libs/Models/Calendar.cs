using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Calendar : BaseModel
    {
        #region Properties
        public int? Id { get; set; }

        public Guid? Key { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Data.Entities.CalendarState State { get; set; }

        public Data.Entities.CriteriaRule CriteriaRule { get; set; }

        public IEnumerable<Event> Events { get; set; }

        public IEnumerable<Criteria> Criteria { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        #endregion
    }
}
