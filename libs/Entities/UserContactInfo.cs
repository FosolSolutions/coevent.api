using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// UserContactInfo class, provides a way to manage the many-to-many relationship between users and contact information.
    /// </summary>
    public class UserContactInfo
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the user associated with the contact information.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the contact information.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact information associated with the user.
        /// </summary>
        public int ContactInfoId { get; set; }

        /// <summary>
        /// get/set - The contact information associated with the user.
        /// </summary>
        public ContactInfo ContactInfo { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserContactInfo object.
        /// </summary>
        public UserContactInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserContactInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="info"></param>
        public UserContactInfo(User user, ContactInfo info)
        {
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.ContactInfoId = info?.Id ?? throw new ArgumentNullException(nameof(info));
            this.ContactInfo = info;
        }
        #endregion
    }
}
