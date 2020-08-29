using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class Participant : BaseReadonlyModel
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
        public Data.Entities.ParticipantState State { get; set; } = Data.Entities.ParticipantState.Enabled;

        /// <summary>
        /// get/set - The foreign key to the user account for this participant.
        /// </summary>
        public int? UserId { get; set; }

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
        public Data.Entities.Gender? Gender { get; set; }

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
        public IList<ContactInfo> ContactInfo { get; set; }

        /// <summary>
        /// get - A collection of attributes for the participant.
        /// </summary>
        public IList<Attribute> Attributes { get; set; }
        #endregion
    }
}
