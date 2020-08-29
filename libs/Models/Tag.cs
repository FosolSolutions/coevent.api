namespace CoEvent.Models
{
    public class Tag : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key, unique category type [Category|...]
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key, unique category value (i.e. Sports)
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
