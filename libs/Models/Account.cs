using CoEvent.Data.Entities;
using System;

namespace CoEvent.Models
{
    public class Account : BaseModel
    {
        #region Properties
        public int? Id { get; set; }

        public Guid? Key { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// get/set - The current state of this account.
        /// </summary>
        public AccountState State { get; set; }

        /// <summary>
        /// get/set - The kind of account [Personal, Business]
        /// </summary>
        public AccountKind Kind { get; set; }

        /// <summary>
        /// get/set - Foreign key to the subscription for this account [Free|...].
        /// </summary>
        public int SubscriptionId { get; set; }
        #endregion
    }
}
