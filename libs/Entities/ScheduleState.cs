namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ScheduleState enum, provides the available states for a schedule.
    /// </summary>
    public enum ScheduleState
    {
        /// <summary>
        /// Closed - this schedule is not visible to participants.
        /// </summary>
        Closed = 0,
        /// <summary>
        /// Published - this schedule is visible to participants.
        /// </summary>
        Published = 1
    }
}
