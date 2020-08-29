namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CalendarState enum, provides the available states for a calendar.
    /// </summary>
    public enum CalendarState
    {
        /// <summary>
        /// Closed - this calendar is not visible to participants.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Published - this calendar is visible to participants.
        /// </summary>
        Published = 1
    }
}
