using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningAnswer class, provides a way to manage participant answers to opening questions.
    /// </summary>
    public class OpeningAnswer : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary and foreign key to the opening.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening who asked this question.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - The primary and foreign key to the question.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// get/set - The question this answer belongs to.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// get/set - The question this answer belongs to.
        /// </summary>
        public OpeningQuestion OpeningQuestion { get; set; }

        /// <summary>
        /// get/set - The primary and foreign key to the participant.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant who provided this answer.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - The opening participant application record.
        /// </summary>
        public OpeningParticipant OpeningParticipant { get; set; }

        /// <summary>
        /// get/set - The answer text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who added this answer.
        /// </summary>
        public new int? AddedById { get; set; }

        /// <summary>
        /// get - A collection of options selected as an answer to the question.
        /// </summary>
        public ICollection<OpeningAnswerQuestionOption> Options { get; private set; } = new List<OpeningAnswerQuestionOption>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningAnswer object.
        /// </summary>
        public OpeningAnswer()
        { }

        /// <summary>
        /// Creates a new instance of a OpeningAnswer object, and initializes it with the specified property values.
        /// </summary>
        /// <param name="openingQuestion"></param>
        /// <param name="openingParticipant"></param>
        /// <param name="answer"></param>
        public OpeningAnswer(OpeningQuestion openingQuestion, OpeningParticipant openingParticipant, string answer)
        {
            if (String.IsNullOrWhiteSpace(answer)) throw new ArgumentException("The argument 'answer' cannot be null, empty or whitespace.");

            this.QuestionId = openingQuestion?.QuestionId ?? throw new ArgumentNullException(nameof(openingQuestion));
            this.Question = openingQuestion.Question;
            this.OpeningId = openingQuestion?.OpeningId ?? throw new ArgumentNullException(nameof(openingQuestion));
            this.Opening = openingQuestion.Opening;
            this.OpeningQuestion = openingQuestion;
            this.ParticipantId = openingParticipant?.ParticipantId ?? throw new ArgumentNullException(nameof(openingParticipant));
            this.Participant = openingParticipant.Participant;
            this.OpeningParticipant = openingParticipant;
            this.Text = answer;
        }
        #endregion
    }
}
