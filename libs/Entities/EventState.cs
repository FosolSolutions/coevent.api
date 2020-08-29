namespace CoEvent.Data.Entities
{
    /// <summary>
    /// EventState enum, provides the available states for an event.
    /// </summary>
    public enum EventState
    {
        /// <summary>
        /// Closed - This event is not visible to participants.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Published - This event is visible to participants.
        /// </summary>
        Published = 1,
        /// <summary>
        /// Cancelled - This event is visible to participants, but has been cancelled.
        /// </summary>
        Cancelled = 2
    }
}
