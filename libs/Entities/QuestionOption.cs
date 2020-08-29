using System;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// QuestionOption class, provides a way to manage available options to questions.
    /// </summary>
    public class QuestionOption : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent question.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// get/set - The question this option belongs to.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// get/set - The answer or option that will be displayed ot the participant.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get/set - The order to display the options.
        /// </summary>
        public int Sequence { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a QuestionOption object.
        /// </summary>
        public QuestionOption()
        { }

        /// <summary>
        /// Creates new instance of a QuestionOption object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="value"></param>
        /// <param name="sequence"></param>
        public QuestionOption(Question question, string value, int sequence = 0)
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("The argument 'value' cannot be null, empty or whitespace.");
            this.QuestionId = question?.Id ?? throw new ArgumentNullException(nameof(question));
            this.Question = question;
            this.Value = value;
            this.Sequence = sequence;
        }
        #endregion
    }
}
