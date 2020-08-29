namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ActivityState enum, provides the available states of an activity.
    /// </summary>
    public enum ActivityState
    {
        /// <summary>
        /// Closed - The activity is not visible to participants.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Published - The activity is visible to participants.
        /// </summary>
        Published = 1,
        /// <summary>
        /// Cancelled - The activity is visible to participants but has been cancelled.
        /// </summary>
        Cancelled = 2
    }
}
