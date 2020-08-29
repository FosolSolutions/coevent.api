namespace CoEvent.Data.Entities
{
    /// <summary>
    /// AccountState enum, provides the possible states an account can be in.
    /// </summary>
    public enum AccountState
    {
        /// <summary>
        /// This account is disabled.  Uses will not be able to login to this account.
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// This account is enabled.
        /// </summary>
        Enabled = 1,

        /// <summary>
        /// This account has been locked.  The account will require an action by the owner to enable.
        /// </summary>
        Locked = 2
    }
}
