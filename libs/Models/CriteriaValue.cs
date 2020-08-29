namespace CoEvent.Models
{
    public class CriteriaValue
    {
        #region Properties
        /// <summary>
        /// get/set - The logical operator to perform against the criteria.
        /// </summary>
        public Data.Entities.LogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// get/set - A unique key to identify the criteria.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// get/set - The value of the criteria (i.e. if Key=Gender, Value=Male).
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// get/set - The type of data in the value.
        /// </summary>
        public string ValueType { get; set; }
        #endregion
    }
}
