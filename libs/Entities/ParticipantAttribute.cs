using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// ParticipantAttribute class, provides a way to manage the many-to-many relationship between participants and attributes.
    /// </summary>
    public class ParticipantAttribute
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant associated with the attribute.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - Foreign key to the attribute.
        /// </summary>
        public int AttributeId { get; set; }

        /// <summary>
        /// get/set - The attribute associated with the participant.
        /// </summary>
        public Attribute Attribute { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ParticipantAttribute object.
        /// </summary>
        public ParticipantAttribute()
        {

        }

        /// <summary>
        /// Creates a new instance of a ParticipantAttribute object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="participant"></param>
        /// <param name="attribute"></param>
        public ParticipantAttribute(Participant participant, Attribute attribute)
        {
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
            this.AttributeId = attribute?.Id ?? throw new ArgumentNullException(nameof(attribute));
            this.Attribute = attribute;
        }
        #endregion
    }
}
