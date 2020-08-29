using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CalendarCriteria class, provides a way to manage the many-to-many relationship between calendars and criteria.
    /// </summary>
    public class CalendarCriteria
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the calendar.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar associated with the criteria.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - Foreign key to the criteria.
        /// </summary>
        public int CriteriaId { get; set; }

        /// <summary>
        /// get/set - The criteria associated with the calendar.
        /// </summary>
        public CriteriaObject Criteria { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CalendarCriteria object.
        /// </summary>
        public CalendarCriteria()
        {

        }

        /// <summary>
        /// Creates a new instance of a CalendarCriteria object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="criteria"></param>
        public CalendarCriteria(Calendar calendar, CriteriaObject criteria)
        {
            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.CriteriaId = criteria?.Id ?? throw new ArgumentNullException(nameof(criteria));
            this.Criteria = criteria;
        }
        #endregion
    }
}
