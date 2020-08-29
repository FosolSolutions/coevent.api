using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class Activity : BaseReadonlyModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this activity.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name within an event to identify this activity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this activity.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - When the activity starts.
        /// </summary>
        public DateTime? StartOn { get; set; }

        /// <summary>
        /// get/set - When the activity ends.
        /// </summary>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// get/set - The state of the activity.
        /// </summary>
        public Data.Entities.ActivityState State { get; set; } = Data.Entities.ActivityState.Published;

        /// <summary>
        /// get/set - How criteria is applied to participants in this activity.  This can be overridden in child entities.
        /// </summary>
        public Data.Entities.CriteriaRule CriteriaRule { get; set; } = Data.Entities.CriteriaRule.Participate;

        /// <summary>
        /// get/set - The order the activities will be displayed.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// get - A collection of openings within this activity.
        /// </summary>
        public IList<Opening> Openings { get; set; }

        /// <summary>
        /// get - A collection of criteria within this activity.
        /// </summary>
        public IList<Criteria> Criteria { get; set; }

        /// <summary>
        /// get - A collection of tags for this activity.
        /// </summary>
        public IList<Tag> Tags { get; set; }
        #endregion
    }
}
