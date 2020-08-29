using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models
{
    public class Schedule : BaseModel
    {
        #region Properties
        public int? Id { get; set; }
        public Guid? Key { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartOn { get; set; }
        public DateTime EndOn { get; set; }

        public Data.Entities.ScheduleState State { get; set; }

        public Data.Entities.CriteriaRule CriteriaRule { get; set; }

        public IList<Event> Events { get; set; }
        #endregion
    }
}
