using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// AccountPrivilege enum, provides the access levels to the account.
    /// </summary>
    [Flags]
    public enum AccountPrivilege
    {
        /// <summary>
        /// This privilege gives basic access to the account.
        /// </summary>
        Basic = 0,

        /// <summary>
        /// This privilege gives administrator access to the account.
        /// </summary>
        Administrator = 1
    }
}
