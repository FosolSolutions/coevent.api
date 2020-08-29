using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    /// <summary>
    /// Opening class, provides a way to manage openings within activities, that participants can apply to.
    /// </summary>
    public class Opening : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - A unique id to identify this opening.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// get/set - The parent activity that owns this opening.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this opening.
        /// </summary>
        public Guid? Key { get; set; }

        /// <summary>
        /// get/set - A name to identify this opening.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the opening.
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
        /// get/set - The current state of the opening.
        /// </summary>
        public Data.Entities.OpeningState State { get; set; }

        /// <summary>
        /// get/set - How the criteria will be enforced.
        /// </summary>
        public Data.Entities.CriteriaRule CriteriaRule { get; set; }

        /// <summary>
        /// get/set - The collection of participants in this opening.  These are accepted applications.
        /// </summary>
        public IList<Participant> Participants { get; set; }

        /// <summary>
        /// get/set - The collection of applications for this opening.
        /// </summary>
        public IList<OpeningApplication> Applications { get; set; }

        /// <summary>
        /// get/set - The criteria for this opening.
        /// </summary>
        public IList<Criteria> Criteria { get; set; }

        /// <summary>
        /// get/set - Tags to filter this opening.
        /// </summary>
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// get/set - The collection of questions for applying to this opening.
        /// </summary>
        public IList<Question> Questions { get; set; }
        #endregion
    }
}
