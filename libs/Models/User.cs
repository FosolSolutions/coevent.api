using CoEvent.Data.Entities;
using System;
using System.Collections.Generic;

namespace CoEvent.Models
{
    public class User : BaseModel
    {
        #region Properties
        public int? Id { get; set; }

        public Guid? Key { get; set; }

        public string Email { get; set; }

        public int? DefaultAccountId { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public Gender? Gender { get; set; }

        public DateTime? Birthdate { get; set; }

        public Account DefaultAccount { get; set; }

        /// <summary>
        /// get/set - The user's home address.
        /// </summary>
        public Address HomeAddress { get; set; }

        /// <summary>
        /// get/set - The users' work address.
        /// </summary>
        public Address WorkAddress { get; set; }

        /// <summary>
        /// get/set - The participants home phone.
        /// </summary>
        public PhoneNumber HomePhone { get; set; }

        /// <summary>
        /// get/set - The participants mobile phone.
        /// </summary>
        public PhoneNumber MobilePhone { get; set; }

        /// <summary>
        /// get/set - The participants work phone.
        /// </summary>
        public PhoneNumber WorkPhone { get; set; }

        public IList<Account> OwnedAccounts { get; set; }

        public IList<Account> Accounts { get; set; }

        public IList<Attribute> Attributes { get; set; }

        public IList<OauthAccount> OauthAccounts { get; set; }
        #endregion
    }
}
