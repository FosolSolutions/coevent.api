using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// OpeningQuestion class, provides a way to link questions to openings.
    /// </summary>
    public class OpeningQuestion
    {
        #region Properties
        /// <summary>
        /// get/set - Primary and foreign key to the parent opening.
        /// </summary>
        public int OpeningId { get; set; }

        /// <summary>
        /// get/set - The parent opening.
        /// </summary>
        public Opening Opening { get; set; }

        /// <summary>
        /// get/set - Primary and foreign key to the question that belongs to the opening.
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// get/set - The question that belongs to the opening.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// get - A collection of answers for this question.
        /// </summary>
        public ICollection<OpeningAnswer> Answers { get; private set; } = new List<OpeningAnswer>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a OpeningQuestion object.
        /// </summary>
        public OpeningQuestion()
        { }

        /// <summary>
        /// Creates a new instance of a OpeningQuestion object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="opening"></param>
        /// <param name="question"></param>
        public OpeningQuestion(Opening opening, Question question)
        {
            this.OpeningId = opening?.Id ?? throw new ArgumentNullException(nameof(opening));
            this.Opening = opening;
            this.QuestionId = question?.Id ?? throw new ArgumentNullException(nameof(question));
            this.Question = question;
        }
        #endregion
    }
}
