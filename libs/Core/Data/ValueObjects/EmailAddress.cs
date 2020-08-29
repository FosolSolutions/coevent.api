using System.Collections.Generic;

namespace CoEvent.Core.Data.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        #region Properties
        /// <summary>
        /// get/set - The phone number.
        /// </summary>
        public string Address { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a EmailAddress object.
        /// </summary>
        public EmailAddress()
        {

        }

        public EmailAddress(string email)
        {
            this.Address = email;
        }
        #endregion

        #region Methods
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Address;
        }
        #endregion
    }
}
