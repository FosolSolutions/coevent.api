namespace CoEvent.Data.Entities
{
    /// <summary>
    /// EventRepetition enum, provides a way to control how often an event repeats.
    /// </summary>
    public enum EventRepetition
    {
        /// <summary>
        /// None - This event does not repeat.
        /// </summary>
        None = 0,
        /// <summary>
        /// Daily - This event repeats every day.
        /// </summary>
        Daily = 1,
        /// <summary>
        /// Weekly - This event repeats every week.
        /// </summary>
        Weekly = 2,
        /// <summary>
        /// Monthly - This event repeats every month.
        /// </summary>
        Monthly = 3,
        /// <summary>
        /// Yearly - This event repeats every year.
        /// </summary>
        Yearly = 4,
        /// <summary>
        /// EndOfMonth - This event repeats at the end of every month.
        /// </summary>
        EndOfMonth = 5,
        /// <summary>
        /// BiWeekly - This event repeats every second week.
        /// </summary>
        BiWeekly = 6,
        /// <summary>
        /// BiMonth - This event repeats every second month.
        /// </summary>
        BiMonthly = 7,
        /// <summary>
        /// Custom - This event repeats based on a custom pattern.
        /// </summary>
        Custom = 8
    }
}
