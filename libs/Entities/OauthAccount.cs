using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OauthAccount class, provides a way to manage external oauth user accounts and associate them with an internal user.
    /// </summary>
    public class OauthAccount : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and foreign key to the owning User.  This provides a way to associate oauth accounts to internal users.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The internal user account that owns this oauth user account.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - A unique email for this oauth user.  Also the primary key.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get/set - a unique key to identify this oauth account user.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The issuer of this account (i.e. [Microsoft, Google, Facebook]).
        /// </summary>
        public string Issuer { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an OauthAccount object.
        /// </summary>
        public OauthAccount()
        {

        }

        /// <summary>
        /// Creates a new instance of an OauthAccount object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <param name="issuer"></param>
        /// <param name="key"></param>
        public OauthAccount(User user, string email, string issuer, string key)
        {
            if (String.IsNullOrWhiteSpace(email)) throw new ArgumentException($"Argument 'email' cannot be null, empty or whitespace.");
            if (String.IsNullOrWhiteSpace(issuer)) throw new ArgumentException($"Argument 'issuer' cannot be null, empty or whitespace.");
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException($"Argument 'key' cannot be null, empty or whitespace.");

            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.Email = email;
            this.Issuer = issuer;
            this.Key = key;
        }
        #endregion
    }
}
