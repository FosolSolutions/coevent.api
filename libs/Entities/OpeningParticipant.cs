using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningParticipant class, provides a way to manage the many-to-many relationship between openings and participants.
    /// </summary>
    public class OpeningParticipant : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Foreign key to the opening associated with the role.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening associated with the role.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact information associated with the opening.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The contact information associated with the opening.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - The state of this application.
        /// </summary>
        public OpeningApplicationState State { get; set; } = OpeningApplicationState.Applied;

        /// <summary>
        /// get/set - The user who added this participant to the opening.
        /// </summary>
        public new int? AddedById { get; set; }

        /// <summary>
        /// get - A collection of answers to the opening questions.
        /// </summary>
        public ICollection<OpeningAnswer> Answers { get; private set; } = new List<OpeningAnswer>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a OpeningParticipant object.
        /// </summary>
        public OpeningParticipant()
        {

        }

        /// <summary>
        /// Creates a new instance of a OpeningParticipant object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="participant"></param>
        /// <param name="state"></param>
        public OpeningParticipant(Opening opening, Participant participant, OpeningApplicationState state = OpeningApplicationState.Applied)
        {
            this.OpeningId = opening?.Id ?? throw new ArgumentNullException(nameof(opening));
            this.Opening = opening;
            this.ParticipantId = participant?.Id ?? throw new ArgumentNullException(nameof(participant));
            this.Participant = participant;
            this.State = state;
        }
        #endregion
    }
}
