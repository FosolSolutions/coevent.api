namespace CoEvent.Models
{
    public class Attribute : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - Primary key.  A unique way to identify a attribute.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - Primary key.  A value to specify the attribute.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get/set - The datatype of the attribute.
        /// </summary>
        public string ValueType { get; set; }
        #endregion
    }
}
