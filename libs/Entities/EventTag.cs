using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// EventTag class, provides a way to manage the many-to-many relationship between events and tags.
    /// </summary>
    public class EventTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the event associated with the tag.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// get/set - The event associated with the tag.
        /// </summary>
        public Event Event { get; set; }

        /// <summary>
        /// get/set - The tag key associated with the event.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The tag value associated with the event.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a EventTag object.
        /// </summary>
        public EventTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a EventTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public EventTag(Event calendarEvent, string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Argument 'key' cannot be null, empty or whitespace.");
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument 'value' cannot be null, empty or whitespace.");
            this.EventId = calendarEvent?.Id ?? throw new ArgumentNullException(nameof(calendarEvent));
            this.Event = calendarEvent;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
