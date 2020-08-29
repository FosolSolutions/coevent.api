using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class Opening : BaseReadonlyModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this opening.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A name to identify this opening.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description about this opening.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The minimum number of participants required for this opening.
        /// </summary>
        public int MinParticipants { get; set; }

        /// <summary>
        /// get/set - The maximum number of participants allowed in this opening.
        /// </summary>
        public int MaxParticipants { get; set; }

        /// <summary>
        /// get/set - The type of opening.
        /// </summary>
        public Data.Entities.OpeningType OpeningType { get; set; }

        /// <summary>
        /// get/set - The process participants can apply for this opening.
        /// </summary>
        public Data.Entities.ApplicationProcess ApplicationProcess { get; set; }

        /// <summary>
        /// get/set - The state of the opening.
        /// </summary>
        public Data.Entities.OpeningState State { get; set; } = Data.Entities.OpeningState.Published;

        /// <summary>
        /// get/set - How criteria is applied to participants in this opening.  This can be overridden in child entities.
        /// </summary>
        public Data.Entities.CriteriaRule CriteriaRule { get; set; } = Data.Entities.CriteriaRule.Participate;

        /// <summary>
        /// get/set - A collection of participants that have been accepted to the opening.
        /// </summary>
        public IList<Participant> Participants { get; set; }

        /// <summary>
        /// get/set - A collection of criteria for this opening.
        /// </summary>
        public IList<Criteria> Criteria { get; set; }

        /// <summary>
        /// get - A collection of tags for this opening.
        /// </summary>
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// get - A collection of questions asked when a participant applies for this opening.
        /// </summary>
        public IList<Question> Questions { get; set; }
        #endregion
    }
}
