using System;
using System.Collections.Generic;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Subscription class, provides a way to manage subscription information in the datasource.
    /// </summary>
    public class Subscription : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENTITY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this subscription.
        /// </summary>
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - A unique name to identify the subscription.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of this subscription.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The current state of this subscription.
        /// </summary>
        public SubscriptionState State { get; set; } = SubscriptionState.Enabled;

        /// <summary>
        /// get/set - Foreign key do the user who added this subscription.
        /// </summary>
        public new int? AddedById { get; set; }

        /// <summary>
        /// get - A Collection of accounts using this subscription.
        /// </summary>
        public ICollection<Account> Accounts { get; private set; } = new List<Account>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a Subscription object.
        /// </summary>
        public Subscription()
        { }

        /// <summary>
        /// Creates a new instance of a Subscription object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public Subscription(string name, string description)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrWhiteSpace(description))
                throw new ArgumentNullException(nameof(description));

            this.Name = name;
            this.Description = description;
            this.Key = Guid.NewGuid();
        }
        #endregion
    }
}
