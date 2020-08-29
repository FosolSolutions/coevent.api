using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ActivityCriteria class, provides a way to manage the many-to-many relationship between activities and criteria.
    /// </summary>
    public class ActivityCriteria
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the activity.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// get/set - The activity associated with the criteria.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// get/set - Foreign key to the criteria.
        /// </summary>
        public int CriteriaId { get; set; }

        /// <summary>
        /// get/set - The criteria associated with the activity.
        /// </summary>
        public CriteriaObject Criteria { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityCriteria object.
        /// </summary>
        public ActivityCriteria()
        {

        }

        /// <summary>
        /// Creates a new instance of a ActivityCriteria object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="criteria"></param>
        public ActivityCriteria(Activity activity, CriteriaObject criteria)
        {
            this.ActivityId = activity?.Id ?? throw new ArgumentNullException(nameof(activity));
            this.Activity = activity;
            this.CriteriaId = criteria?.Id ?? throw new ArgumentNullException(nameof(criteria));
            this.Criteria = criteria;
        }
        #endregion
    }
}
