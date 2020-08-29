using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// UserAccountRole class, provides a way to manage the many-to-many relationship between users and account roles.
    /// </summary>
    public class UserAccountRole
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the user associated with the role.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the role.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the role associated with the user.
        /// </summary>
        public int AccountRoleId { get; set; }

        /// <summary>
        /// get/set - The account role associated with the user.
        /// </summary>
        public AccountRole AccountRole { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserAccountRole object.
        /// </summary>
        public UserAccountRole()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserAccountRole object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        public UserAccountRole(User user, AccountRole role)
        {
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.AccountRoleId = role?.Id ?? throw new ArgumentNullException(nameof(role));
            this.AccountRole = role;
        }
        #endregion
    }
}
