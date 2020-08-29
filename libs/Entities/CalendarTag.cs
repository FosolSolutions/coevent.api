using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CalendarTag class, provides a way to manage the many-to-many relationship between calendars and tags.
    /// </summary>
    public class CalendarTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the calendar associated with the tag.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar associated with the tag.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - The tag key associated with the calendar.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The tag value associated with the calendar.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a CalendarTag object.
        /// </summary>
        public CalendarTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a CalendarTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarCalendar"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public CalendarTag(Calendar calendarCalendar, string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Argument 'key' cannot be null, empty or whitespace.");
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument 'value' cannot be null, empty or whitespace.");
            this.CalendarId = calendarCalendar?.Id ?? throw new ArgumentNullException(nameof(calendarCalendar));
            this.Calendar = calendarCalendar;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
