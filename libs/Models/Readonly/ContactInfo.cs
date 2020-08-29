using System;
using System.Collections.Generic;
using System.Text;

namespace CoEvent.Models.Readonly
{
    public class ContactInfo : BaseReadonlyModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique name to identify this contact information.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The type of contact information.
        /// </summary>
        public Data.Entities.ContactInfoType Type { get; set; }

        /// <summary>
        /// get/set - Categorizes the contact information.
        /// </summary>
        public Data.Entities.ContactInfoCategory Category { get; set; }

        /// <summary>
        /// get/set - The email address, phone number of other other.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}
