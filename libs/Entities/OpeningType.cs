using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningType enum, opening type options control the behavior of the opening.
    /// </summary>
    public enum OpeningType
    {
        /// <summary>
        /// Application - The participant must apply.
        /// </summary>
        Application = 0,
        /// <summary>
        /// Invitation - The participant must be invited.
        /// </summary>
        Invitation = 1
    }
}
