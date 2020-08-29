namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ActionTrigger enum, provides the action events that can occur within an opening.
    /// </summary>
    public enum ActionTrigger
    {
        /// <summary>
        /// Publish - The opening was published.
        /// </summary>
        Publish = 0,
        /// <summary>
        /// Apply - A participant has applied to the opening.
        /// </summary>
        Apply = 1,
        /// <summary>
        /// Unapply - A participant has removed their application.
        /// </summary>
        Unapply = 2,
        /// <summary>
        /// Accept - An application was accepted.
        /// </summary>
        Accept = 3,
        /// <summary>
        /// Reject - An application was rejected.
        /// </summary>
        Reject = 4
    }
}
