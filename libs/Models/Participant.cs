using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Participant : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this participant.
        /// </summary>
        public Guid? Key { get; set; }

        /// <summary>
        /// get/set - The state of this participant.
        /// </summary>
        public Data.Entities.ParticipantState State { get; set; }

        /// <summary>
        /// get/set - The foreign key to the user account for this participant.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the calendar this participant belongs to.
        /// </summary>
        public int CalendarId { get; set; }

        /// <summary>
        /// get/set - A participants name to display for others to see.  This should be unique within each Calendar.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// get/set - The participants default email address.
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
        public Data.Entities.Gender? Gender { get; set; }

        /// <summary>
        /// get/set - The participants birthdate.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// get/set - The participants home address.
        /// </summary>
        public Address HomeAddress { get; set; }

        /// <summary>
        /// get/set - The participants work address.
        /// </summary>
        public Address WorkAddress { get; set; }

        /// <summary>
        /// get/set - The participants home phone.
        /// </summary>
        public PhoneNumber HomePhone { get; set; }

        /// <summary>
        /// get/set - The participants mobile phone.
        /// </summary>
        public PhoneNumber MobilePhone { get; set; }

        /// <summary>
        /// get/set - The participants work phone.
        /// </summary>
        public PhoneNumber WorkPhone { get; set; }

        /// <summary>
        /// get - A collection of information about the participant.
        /// </summary>
        public IList<ContactInfo> ContactInfo { get; set; }

        /// <summary>
        /// get - A collection of attributes for the participant.
        /// </summary>
        public IList<Attribute> Attributes { get; set; }
        #endregion
    }
}
