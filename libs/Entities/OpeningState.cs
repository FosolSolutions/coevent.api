namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningState enum, provides the available states for openings.
    /// </summary>
    public enum OpeningState
    {
        /// <summary>
        /// Closed - The opening is not visible to participants.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Published - The opening is visible to participants.
        /// </summary>
        Published = 1,
        /// <summary>
        /// Cancelled - The opening is visible to participants, but has been cancelled.
        /// </summary>
        Cencelled = 2
    }
}
