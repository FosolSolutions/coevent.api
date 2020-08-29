using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ParticipantContactInfo class, provides a way to manage the many-to-many relationship between participants and contact information.
    /// </summary>
    public class ParticipantContactInfo
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the participant associated with the contact information.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant associated with the contact information.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact information associated with the participant.
        /// </summary>
        public int ContactInfoId { get; set; }

        /// <summary>
        /// get/set - The contact information associated with the participant.
        /// </summary>
        public ContactInfo ContactInfo { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a ParticipantContactInfo object.
        /// </summary>
        public ParticipantContactInfo()
        {

        }

        /// <summary>
        /// Creates a new instance of a ParticipantContactInfo object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="info"></param>
        public ParticipantContactInfo(Participant participant, ContactInfo info)
        {
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
            this.ContactInfoId = info?.Id ?? throw new ArgumentNullException(nameof(info));
            this.ContactInfo = info;
        }
        #endregion
    }
}
