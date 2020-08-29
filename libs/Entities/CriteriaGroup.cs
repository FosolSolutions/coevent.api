using CoEvent.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// CriteriaGroup class, provides a way to manage a group of criteria.
    /// </summary>
    public class CriteriaGroup : Criteria
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A collection of criteria.
        /// </summary>
        public IList<Criteria> Criteria { get; set; } = new List<Criteria>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a CriteriaGroup object.
        /// </summary>
        public CriteriaGroup()
        {

        }

        /// <summary>
        /// Creates a new instance of a CriteriaGroup object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="logicalOperator"></param>
        /// <param name="criteria"></param>
        public CriteriaGroup(LogicalOperator logicalOperator, params Criteria[] criteria)
        {
            this.LogicalOperator = LogicalOperator;
            criteria.AddRange(criteria);
        }

        /// <summary>
        /// Creates a new instance of a CriteriaGroup object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="criteria"></param>
        public CriteriaGroup(CriteriaObject criteria)
        {
            this.Id = criteria.Id;
            var values = criteria.Statement.Split(';');
            values.ForEach(c => this.Criteria.Add(new CriteriaValue(c)));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Validates that the attribute(s) match the criteria.
        /// Currently this is a simple check; one OR must pass and all AND must pass.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override bool Validate(params Attribute[] attributes) // TODO: Handle complex grouping.
        {
            var results = this.Criteria.Select(c => new Tuple<LogicalOperator, bool>(c.LogicalOperator, c.Validate(attributes)));

            var validates = true;
            foreach (var pass in results)
            {
                if (pass.Item1 == LogicalOperator.And && !pass.Item2) validates = false; // Any failed AND will not validate.
                else if (pass.Item1 == LogicalOperator.Or && pass.Item2) validates = true; // Any passed OR will validate.
            }

            return validates;
        }

        /// <summary>
        /// Converts the criteria group into a string value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        /// <summary>
        /// Converts the criteria group into a string value and encodes it.
        /// </summary>
        /// <param name="encode"></param>
        /// <returns></returns>
        public override string ToString(bool encode)
        {
            return String.Join(';', this.Criteria.Select(c => c.ToString(encode)));
        }
        #endregion
    }
}
