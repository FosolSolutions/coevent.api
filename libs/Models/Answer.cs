using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Answer : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary and foreign key to the opening.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The primary and foreign key to the question.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// get/set - The primary and foreign key to the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The answer text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// get - A collection of options selected as an answer to the question.
        /// </summary>
        public IList<QuestionOption> Options { get; set; }
        #endregion
    }
}
