using System.Collections.Generic;

namespace CoEvent.Core.Data.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        #region Properties
        /// <summary>
        /// get/set - A unique name to identify the phone number.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The phone number.
        /// </summary>
        public string Number { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PhoneNumber object.
        /// </summary>
        public PhoneNumber()
        {

        }

        public PhoneNumber(string name, string number)
        {
            this.Name = name;
            this.Number = number;
        }
        #endregion

        #region Methods
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Name;
            yield return this.Number;
        }
        #endregion
    }
}
