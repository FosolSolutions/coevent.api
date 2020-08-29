namespace CoEvent.Data.Entities
{
    /// <summary>
    /// LogicalOperator enum, provides the logical operations that can be performed on the criteria.
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        /// And - This criteria must return true.
        /// </summary>
        And = 0,
        /// <summary>
        /// Or - The previous criteria or this criteria must return true.
        /// </summary>
        Or = 1,
        /// <summary>
        /// Xor - Either this or the previous must return true, but not both.
        /// </summary>
        Xor = 2
    }
}
