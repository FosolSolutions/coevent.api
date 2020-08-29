namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningApplicationState enum, provides the available states for a participant application to an opening.
    /// </summary>
    public enum OpeningApplicationState
    {
        /// <summary>
        /// Applied - The participant has applied to the opening.
        /// </summary>
        Applied = 0,
        /// <summary>
        /// Accepted - The participant application has been accepted.
        /// </summary>
        Accepted = 1,
        /// <summary>
        /// Denied - The participant application has been denied.
        /// </summary>
        Denied = 2
    }
}
