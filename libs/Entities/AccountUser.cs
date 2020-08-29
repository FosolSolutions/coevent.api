using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// AccountUser class, provides a way to manage the many-to-many relationship with accounts and users.
    /// </summary>
    public class AccountUser
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account associated with the user.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the account.
        /// </summary>
        public User User { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AccountUser object.
        /// </summary>
        public AccountUser()
        {

        }

        /// <summary>
        /// Creates a new instance of an AccountUser object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="user"></param>
        public AccountUser(Account account, User user)
        {
            this.AccountId = account?.Id ?? throw new ArgumentNullException(nameof(account));
            this.Account = account;
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
        }
        #endregion
    }
}
