using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Activity : BaseModel
    {
        #region Properties
        public int? Id { get; set; }

        public Guid? Key { get; set; }

        public string Name { get; set; }

        public int EventId { get; set; }

        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public Data.Entities.ActivityState State { get; set; }

        public Data.Entities.CriteriaRule CriteriaRule { get; set; }

        public int Sequence { get; set; }

        public IEnumerable<Opening> Openings { get; set; }

        public IEnumerable<Criteria> Criteria { get; set; }

        public IEnumerable<Tag> Tags { get; set; }
        #endregion
    }
}
