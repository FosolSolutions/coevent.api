namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CriteriaRule enum, provides how criteria is applied to the entity.
    /// </summary>
    public enum CriteriaRule
    {
        /// <summary>
        /// Participate - Whether a participant is allowed to participate.  If the participant does not pass the criteria, they will not be able to apply to the entity.  The entity will remain visible for them to view.  This is the default.
        /// </summary>
        Participate = 0,
        /// <summary>
        /// Visibility - Whether the entity is visible to the participant.  If the participant does not pass the criteria, they will not be able to view the entity.
        /// </summary>
        Visibility = 1
    }
}
