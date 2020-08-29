using System;
using System.Linq;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CriteriaObject class, provides an object within the datasource that represents a criteria.
    /// </summary>
    public class CriteriaObject : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - The criteria statement.
        /// </summary>
        public string Statement { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CriteriaObject object.
        /// </summary>
        public CriteriaObject()
        {
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public CriteriaObject(string key, string value, Type type) : this(LogicalOperator.And, key, value, type)
        {
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public CriteriaObject(LogicalOperator logicalOperator, string key, string value, Type type)
        {
            this.Statement = new CriteriaValue(logicalOperator, key, value, type).ToString(true);
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public CriteriaObject(string key, object value) : this(LogicalOperator.And, key, value)
        {
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public CriteriaObject(LogicalOperator logicalOperator, string key, object value)
        {
            this.Statement = new CriteriaValue(logicalOperator, key, value).ToString(true);
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaObject(CriteriaValue criteria)
        {
            this.Id = criteria.Id;
            this.Statement = criteria.ToString();
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaObject(CriteriaGroup criteria)
        {
            this.Id = criteria.Id;
            this.Statement = criteria.ToString(true);
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaObject(params Criteria[] criteria)
        {
            this.Statement = String.Join(";", criteria.Select(c => c.ToString(true)));
        }

        /// <summary>
        /// Creates a new instance of a CriteriaObject object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaObject(CriteriaObject criteria)
        {
            this.Statement = criteria.ToString(true);
        }
        #endregion

        #region Methods
        public static explicit operator Criteria(CriteriaObject criteria)
        {
            return criteria.Statement.Contains(';') ? new CriteriaGroup(criteria) : new CriteriaValue(criteria) as Criteria;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool encode)
        {
            var criteria = this.Statement.Contains(';') ? new CriteriaGroup(this) : new CriteriaValue(this) as Criteria;
            return criteria?.ToString(encode);
        }
        #endregion
    }
}
