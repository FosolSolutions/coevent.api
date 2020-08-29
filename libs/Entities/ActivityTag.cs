using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ActivityTag class, provides a way to manage the many-to-many relationship between activitys and tags.
    /// </summary>
    public class ActivityTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the activity associated with the tag.
        /// </summary>
        public int ActivityId { get; set; }

        /// <summary>
        /// get/set - The activity associated with the tag.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// get/set - The tag key associated with the activity.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The tag value associated with the activity.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a ActivityTag object.
        /// </summary>
        public ActivityTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a ActivityTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarActivity"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ActivityTag(Activity calendarActivity, string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Argument 'key' cannot be null, empty or whitespace.");
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument 'value' cannot be null, empty or whitespace.");
            this.ActivityId = calendarActivity?.Id ?? throw new ArgumentNullException(nameof(calendarActivity));
            this.Activity = calendarActivity;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
