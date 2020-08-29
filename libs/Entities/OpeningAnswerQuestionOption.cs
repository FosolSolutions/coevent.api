using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningAnswerQuestionOption class, provides a way to manage the participants selected options to the opening questions.
    /// </summary>
    public class OpeningAnswerQuestionOption
    {
        #region Properties
        /// <summary>
        /// get/set - Primary and foreign key to the opening this answer option belongs.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The opening this answer option belongs.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Primary and foreign key to the question this answer option belongs.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// get/set - The question this answer option belongs.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// get/set - Primary and foreign key to the participant this answer option belongs.
        /// </summary>
        public int ParticipantId { get; set; }

        /// <summary>
        /// get/set - The participant this answer option belongs.
        /// </summary>
        public Participant Participant { get; set; }

        /// <summary>
        /// get/set - The opening answer this option belongs.
        /// </summary>
        public OpeningAnswer OpeningAnswer { get; set; }

        /// <summary>
        /// get/set - Primary and foreign key to the question anwser option.
        /// </summary>
        public int QuestionOptionId { get; set; }

        /// <summary>
        /// get/set - The question answer option.
        /// </summary>
        public QuestionOption Option { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an OpeningAnswerQuestionOption object.
        /// </summary>
        public OpeningAnswerQuestionOption()
        { }

        /// <summary>
        /// Creates a new instance of an OpeningAnswerQuestionOption object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="option"></param>
        public OpeningAnswerQuestionOption(OpeningAnswer answer, QuestionOption option)
        {
            if (answer == null) throw new ArgumentNullException(nameof(answer));
            this.OpeningId = answer?.OpeningId ?? throw new ArgumentNullException(nameof(OpeningAnswer.OpeningId));
            this.Opening = answer.Opening;
            this.QuestionId = answer?.QuestionId ?? throw new ArgumentNullException(nameof(OpeningAnswer.QuestionId));
            this.Question = answer.Question;
            this.ParticipantId = answer?.ParticipantId ?? throw new ArgumentNullException(nameof(OpeningAnswer.ParticipantId));
            this.Participant = answer.Participant;
            this.QuestionOptionId = option?.Id ?? throw new ArgumentNullException(nameof(option));
            this.Option = option;
        }
        #endregion
    }
}
