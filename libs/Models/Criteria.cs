using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models
{
    public class Criteria
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - When this is a group it can have its own logical operator.
        /// </summary>
        public Data.Entities.LogicalOperator? LogicalOperator { get; set; }

        /// <summary>
        /// get/set - A collection of criteria conditions.
        /// </summary>
        public IList<CriteriaValue> Conditions { get; set; }
        #endregion
    }
}
