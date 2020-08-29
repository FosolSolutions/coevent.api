using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Question class, provides a way to manage questions asked to participants.
    /// </summary>
    public class Question : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key used IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent account.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// get/set - The account that owns this question.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// get/set - A shortform display for this question.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// get/set - The question that will be asked to the participant.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// get/set - The type of answer.
        /// </summary>
        public AnswerType AnswerType { get; set; }

        /// <summary>
        /// get/set - Whether this question is required.
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// get/set - The sequence to order the questions.
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// get/set - The maximum length of a answer allowed.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// get/set - Whether to allow the participant to enter an 'other' answer.
        /// </summary>
        public bool AllowOther { get; set; }

        /// <summary>
        /// get - A collection of options that can be used as answer(s).
        /// </summary>
        public ICollection<QuestionOption> Options { get; private set; } = new List<QuestionOption>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Question object.
        /// </summary>
        public Question()
        {

        }

        /// <summary>
        /// Creates a new instance of a Question object, and initializes it with the specified property values.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="caption"></param>
        /// <param name="question"></param>
        /// <param name="answerType"></param>
        /// <param name="isRequired"></param>
        /// <param name="maxLength"></param>
        /// <param name="allowOther"></param>
        /// <param name="sequence"></param>
        public Question(Account account, string caption, string question, AnswerType answerType = AnswerType.String, bool isRequired = true, int maxLength = 0, bool allowOther = false, int sequence = 0)
        {
            if (string.IsNullOrWhiteSpace(caption)) throw new ArgumentException("The argument 'caption' cannot be null, empty or whitespace.");
            if (string.IsNullOrWhiteSpace(question)) throw new ArgumentException("The argument 'question' cannot be null, empty or whitespace.");
            this.AccountId = account?.Id ?? throw new ArgumentNullException(nameof(account));
            this.Account = account;
            this.Caption = caption;
            this.Text = question;
            this.AnswerType = AnswerType;
            this.IsRequired = isRequired;
            this.MaxLength = maxLength;
            this.AllowOther = allowOther;
            this.Sequence = sequence;
        }
        #endregion
    }
}
