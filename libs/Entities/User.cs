using CoEvent.Core.Data.ValueObjects;
using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// User class, provides a way to manage user information in the datasource.
    /// </summary>
    public class User : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this user.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique user name to identify this user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// get/set - The user's password hash.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// get/set - A unique email address that identifies this user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - The current state of this user.
        /// </summary>
        public UserState State { get; set; } = UserState.Enabled;

        /// <summary>
        /// get/set - The user information.
        /// </summary>
        public UserInfo Info { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who added this subscription.
        /// </summary>
        public new int? AddedById { get; set; }

        /// <summary>
        /// get/set - Foreign key to the default account this user signs into.
        /// </summary>
        public int? DefaultAccountId { get; set; }

        /// <summary>
        /// get/set - The default account this user signs into.
        /// </summary>
        public Account DefaultAccount { get; set; }

        /// <summary>
        /// get - A collection of all the acounts owned by this user.
        /// </summary>
        public ICollection<Account> OwnedAccounts { get; private set; } = new List<Account>();

        /// <summary>
        /// get - A collection of all the accounts this user is associated with.
        /// </summary>
        public ICollection<AccountUser> Accounts { get; private set; } = new List<AccountUser>();

        /// <summary>
        /// get - A collection of all the roles this user is part of.
        /// </summary>
        public ICollection<UserAccountRole> Roles { get; private set; } = new List<UserAccountRole>();

        /// <summary>
        /// get - A collection of all the participants associated with this user.
        /// </summary>
        public ICollection<Participant> Participants { get; private set; } = new List<Participant>();

        /// <summary>
        /// get - A collection of user contact information.
        /// </summary>
        public ICollection<UserContactInfo> ContactInformation { get; private set; } = new List<UserContactInfo>();

        /// <summary>
        /// get - A collection of attributes for this user.
        /// </summary>
        public ICollection<UserAttribute> Attributes { get; private set; } = new List<UserAttribute>();

        /// <summary>
        /// get - A collection of settings for this user.
        /// </summary>
        public ICollection<UserSetting> Settings { get; private set; } = new List<UserSetting>();

        /// <summary>
        /// get - A collection of external oauth accounts associated with this user.
        /// </summary>
        public ICollection<OauthAccount> OauthAccounts { get; private set; } = new List<OauthAccount>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a User object.
        /// </summary>
        public User()
        { }

        /// <summary>
        /// Creates a new instance of a User object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="state"></param>
        public User(string email, UserState state = UserState.Enabled)
        {
            if (String.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            this.Key = Guid.NewGuid();
            this.Email = new EmailAddress(email).Address;
            this.Username = email;
            this.State = state;
        }

        /// <summary>
        /// Creates a new instance of a User object, and initializes it with the specified properties and links it to the account.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="account"></param>
        /// <param name="state"></param>
        public User(string email, Account account, UserState state = UserState.Enabled) : this(email, state)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            this.Accounts.Add(new AccountUser(account, this));
        }
        #endregion
    }
}
