using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningTag class, provides a way to manage the many-to-many relationship between openings and tags.
    /// </summary>
    public class OpeningTag
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the opening associated with the tag.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening associated with the tag.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag key associated with the opening.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - Foreign key to the tag value associated with the opening.
        /// </summary>
        public string Value { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a OpeningTag object.
        /// </summary>
        public OpeningTag()
        {

        }

        /// <summary>
        /// Creates a new instance of a OpeningTag object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendarOpening"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public OpeningTag(Opening calendarOpening, string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException("Argument 'key' cannot be null, empty or whitespace.");
            this.OpeningId = calendarOpening?.Id ?? throw new ArgumentNullException(nameof(calendarOpening));
            this.Opening = calendarOpening;
            this.Key = key;
            this.Value = value;
        }
        #endregion
    }
}
