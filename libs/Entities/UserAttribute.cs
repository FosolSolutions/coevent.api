using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// UserAttribute class, provides a way to manage the many-to-many relationship between users and attributes.
    /// </summary>
    public class UserAttribute
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user associated with the attribute.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the attribute.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// get/set - The attribute associated with the user.
        /// </summary>
        public Attribute Attribute { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a UserAttribute object.
        /// </summary>
        public UserAttribute()
        {

        }

        /// <summary>
        /// Creates a new instance of a UserAttribute object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="attribute"></param>
        public UserAttribute(User user, Attribute attribute)
        {
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.AttributeId = attribute?.Id ?? throw new ArgumentNullException(nameof(attribute));
            this.Attribute = attribute;
        }
        #endregion
    }
}
