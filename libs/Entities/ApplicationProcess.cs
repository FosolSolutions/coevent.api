namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ApplicationProcess enum, application process options control the behaviour of an application.
    /// </summary>
    public enum ApplicationProcess
    {
        /// <summary>
        /// AutoAccept - The participant application is immediately accepted.
        /// </summary>
        AutoAccept = 0,

        /// <summary>
        /// Review - The participant application must be reviewed before being accepted.
        /// </summary>
        Review = 1
    }
}
