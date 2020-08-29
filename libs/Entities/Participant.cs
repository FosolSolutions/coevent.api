using CoEvent.Core.Data.ValueObjects;
using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Participant class, provides a way to manage participant information in the datasource.
    /// </summary>
    /// <remarks>
    /// A participant is a way to associate a person with an calendar.
    /// A participant may be owned by a user account, but it could represent a person without a user account.
    /// The goal is to provide a way to manage a participant list of people who may or may not have user accounts within the application.
    /// A participant can be managed by the owner of the calendar, or the user.
    /// </remarks>
    public class Participant : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this participant.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - The state of this participant.
        /// </summary>
        public ParticipantState State { get; set; } = ParticipantState.Enabled;

        /// <summary>
        /// get/set - The foreign key to the user account for this participant.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// get/set - The user account that is represented by this participant.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar this participant belongs to.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - The calendar this participant belongs to.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// get/set - A participants name to display for others to see.  This should be unique within each Calendar.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - An email address that identifies this participant.
        /// </summary>
        public string Email { get; set; }

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
        /// get/set - The participants gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// get/set - The participants birthdate.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// get/set - Foreign key to the home address.
        /// </summary>
        public int? HomeAddressId { get; set; }

        /// <summary>
        /// get/set - The participants home address.
        /// </summary>
        public Address HomeAddress { get; set; }

        /// <summary>
        /// get/set - Foreign key to the work address.
        /// </summary>
        public int? WorkAddressId { get; set; }

        /// <summary>
        /// get/set - The participants work address.
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

        /// <summary>
        /// get - A collection of contact information about the participant.
        /// </summary>
        public ICollection<ParticipantContactInfo> ContactInfo { get; private set; } = new List<ParticipantContactInfo>(); // TODO: Need to limit the number.

        /// <summary>
        /// get - A collection of attributes for the participant.
        /// </summary>
        public ICollection<ParticipantAttribute> Attributes { get; private set; } = new List<ParticipantAttribute>(); // TODO: Need to limit the number.

        /// <summary>
        /// get - A collection of openings this participant has applied to, and/or is participating in.
        /// </summary>
        public ICollection<OpeningParticipant> Openings { get; private set; } = new List<OpeningParticipant>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Participant object.
        /// </summary>
        public Participant()
        { }

        /// <summary>
        /// Craetes a new instance of a Participant object, and initizes it with the specified arguments.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="user"></param>
        public Participant(Calendar calendar, User user)
        {
            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.UserId = user?.Id ?? throw new ArgumentNullException(nameof(user));
            this.User = user;
            this.FirstName = user.Info?.FirstName;
            this.MiddleName = user.Info?.MiddleName;
            this.LastName = user.Info?.LastName;
            this.DisplayName = $"{user.Info?.FirstName} {user.Info?.LastName}"; // TODO: Configure default display name options.
            this.Title = user.Info?.Title;
            this.Gender = user.Info?.Gender;
            this.Birthdate = user.Info?.Birthdate;
            this.Key = user.Key;
            this.Email = user.Email;
            this.HomeAddress = user.Info?.HomeAddress;
            this.HomeAddressId = user.Info?.HomeAddressId;
            this.WorkAddress = user.Info?.WorkAddress;
            this.WorkAddressId = user.Info?.WorkAddressId;
        }

        /// <summary>
        /// Craetes a new instance of a Participant object, and initizes it with the specified arguments.
        /// </summary>
        /// <param name="calendar"></param>
        /// <param name="displayName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="home"></param>
        /// <param name="work"></param>
        public Participant(Calendar calendar, string displayName, string firstName = null, string lastName = null, string email = null, Address home = null, Address work = null)
        {
            if (String.IsNullOrWhiteSpace(displayName))
                throw new ArgumentException($"The argument '{nameof(displayName)}' is required and cannot be null or empty."); // TODO: Use string resource file.

            this.CalendarId = calendar?.Id ?? throw new ArgumentNullException(nameof(calendar));
            this.Calendar = calendar;
            this.DisplayName = displayName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Key = Guid.NewGuid();
            this.Email = new EmailAddress(email).Address;
            this.HomeAddressId = home?.Id;
            this.HomeAddress = home;
            this.WorkAddressId = work?.Id;
            this.WorkAddress = work;
        }
        #endregion
    }
}
