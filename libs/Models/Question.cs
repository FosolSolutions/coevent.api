using System.Collections.Generic;

namespace CoEvent.Models
{
    public class Question : BaseModel
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
        public Data.Entities.AnswerType AnswerType { get; set; }

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
        /// get/set - A collection of options that can be used as answer(s).
        /// </summary>
        public IList<QuestionOption> Options { get; set; }
        #endregion
    }
}
