namespace CoEvent.Models
{
    public class Address
    {
        #region Properties
        /// <summary>
        /// get/set - A unique name to identify this address.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Address line 1.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// get/set - Address line 2.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// get/set - City name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// get/set - State or province name.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// get/set - ZIP or postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// get/set - Country name.
        /// </summary>
        public string Country { get; set; }
        #endregion
    }
}
