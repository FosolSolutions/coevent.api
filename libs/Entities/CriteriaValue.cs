using CoEvent.Core.Extensions;
using System;
using System.Linq;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CriteriaValue class, provides a way to manage a single criteria statement.
    /// </summary>
    public class CriteriaValue : Criteria
    {
        #region Propeties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

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

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CriteriaValue object.
        /// </summary>
        public CriteriaValue()
        {

        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public CriteriaValue(string key, string value, Type type) : this(LogicalOperator.And, key, value, type)
        {
        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public CriteriaValue(LogicalOperator logicalOperator, string key, string value, Type type)
        {
            this.LogicalOperator = LogicalOperator;
            this.Key = key;
            this.Value = value;
            this.ValueType = type.FullName;
        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public CriteriaValue(string key, object value) : this(LogicalOperator.And, key, value)
        {
        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public CriteriaValue(LogicalOperator logicalOperator, string key, object value)
        {
            this.LogicalOperator = LogicalOperator;
            this.Key = key;
            this.Value = $"{value}"; // TODO: serialize.
            this.ValueType = value?.GetType().FullName;
        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaValue(CriteriaObject criteria) : this(criteria.Statement)
        {
            this.Id = criteria.Id;
        }

        /// <summary>
        /// Creates a new instance of a CriteriaValue object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaValue(string criteria)
        {
            var values = criteria.Split(',');
            this.LogicalOperator = Enum.Parse<LogicalOperator>(values[0]);
            this.Key = values[1]; // TODO: decode.
            this.Value = values[2]; // TODO: decode.
            this.ValueType = values[3]; // TODO: handle generics.
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates that the attribute(s) match the criteria.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override bool Validate(params Attribute[] attributes)
        {
            return attributes.Any(a => a.Key.Equals(this.Key) && a.GetValue().Equals(this.GetValue()));
        }

        /// <summary>
        /// Convert this object into a string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Convert this object into a string representation and encode the property values.
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public override string ToString(bool encode)
        {
            // TODO: encode key, value.
            return encode ? $"{this.LogicalOperator},{this.Key},{this.Value},{this.ValueType}" : $"{this.LogicalOperator},{this.Key},{this.Value},{this.ValueType}";
        }

        /// <summary>
        /// Get the type of the value.
        /// </summary>
        /// <returns></returns>
        public Type GetValueType()
        {
            return Type.GetType(this.ValueType);
        }

        /// <summary>
        /// Get the value after converting it to the configured type.
        /// </summary>
        /// <returns></returns>
        public object GetValue()
        {
            // TODO: deserialize objects.
            return this.Value.ConvertTo(this.GetValueType());
        }
        #endregion
    }
}
