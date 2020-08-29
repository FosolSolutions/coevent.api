namespace CoEvent.Models
{
    public class QuestionOption : BaseModel
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
        /// get/set - The answer or option that will be displayed ot the participant.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get/set - The order to display the options.
        /// </summary>
        public int Sequence { get; set; }
        #endregion
    }
}
