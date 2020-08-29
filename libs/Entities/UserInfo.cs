using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// UserInfo class, provides a way to manage user information in the datasource.
    /// </summary>
    public class UserInfo : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key and foreign key to the user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// get/set - The user who this info belongs to.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - The user's display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - The persons title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// get/set - The persons first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The persons middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// get/set - The persons last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// get/set - The persons birthdate.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// get/set - A description of the person.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The perons gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// get/set - Foreign key to the home address.
        /// </summary>
        public int? HomeAddressId { get; set; }

        /// <summary>
        /// get/set - The user's home address.
        /// </summary>
        public Address HomeAddress { get; set; }

        /// <summary>
        /// get/set - Foreign key to the work address.
        /// </summary>
        public int? WorkAddressId { get; set; }

        /// <summary>
        /// get/set - The users' work address.
        /// </summary>
        public Address WorkAddress { get; set; }

        /// <summary>
        /// get/set - The participants home phone.
        /// </summary>
        public string HomePhone { get; set; }

        /// <summary>
        /// get/set - The participants mobile phone.
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// get/set - The participants work phone.
        /// </summary>
        public string WorkPhone { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a UserInfo object.
        /// </summary>
        public UserInfo()
        { }

        /// <summary>
        /// Creates anew instance of a UserInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="displayName"></param>
        public UserInfo(User user, string displayName)
        {
            if (String.IsNullOrWhiteSpace(displayName))
                throw new ArgumentNullException(nameof(displayName));

            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
        }

        /// <summary>
        /// Creates anew instance of a UserInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public UserInfo(User user, string firstName, string lastName) : this(user, $"{firstName} {lastName}")
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        #endregion
    }
}
