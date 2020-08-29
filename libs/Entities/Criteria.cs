namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Criteria class, provides a way to manage criteria in the datasource.
    /// </summary>
    public abstract class Criteria
    {
        #region Properties
        /// <summary>
        /// get/set - The logical operator to perform against the criteria.
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Validate this criteria against the specified attributes.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public abstract bool Validate(params Attribute[] attributes);

        /// <summary>
        /// Converts the criteria expression into a string.
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public virtual string ToString(bool encode)
        {
            return base.ToString();
        }
        #endregion
    }
}
