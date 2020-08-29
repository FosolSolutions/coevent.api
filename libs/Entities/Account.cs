using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoEvent.Data.Entities
{
    /// <summary>
    /// Account class, provides a way to manage account information in the datasource.
    /// </summary>
    public class Account : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key uses IDENITTY.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique key to identify this account.
        /// </summary>
        [Required]
        public Guid Key { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user who owns this account.
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// get/set - The owner of this account.
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// get/set - The current state of this account.
        /// </summary>
        public AccountState State { get; set; } = AccountState.Enabled;

        /// <summary>
        /// get/set - The kind of account [Personal, Business]
        /// </summary>
        public AccountKind Kind { get; set; }

        /// <summary>
        /// get/set - Foreign key to the subscription for this account [Free|...].
        /// </summary>
        public int SubscriptionId { get; set; }

        /// <summary>
        /// get/set - The current subscription for this account.
        /// </summary>
        public Subscription Subscription { get; set; }

        /// <summary>
        /// get/set - Foreign key to the business address.
        /// </summary>
        public int? BusinessAddressId { get; set; }

        /// <summary>
        /// get/set - The account's business address.
        /// </summary>
        public Address BusinessAddress { get; set; }

        /// <summary>
        /// get/set - The account's business phone.
        /// </summary>
        public string BusinessPhone { get; set; }

        /// <summary>
        /// get/set - The account's toll-free phone.
        /// </summary>
        public string TollFreeNumber { get; set; }

        /// <summary>
        /// get/set - The account's fax number.
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// get/set - The account's email adddress.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// get - A collection of users associated with this account.
        /// </summary>
        public ICollection<AccountUser> Users { get; private set; } = new List<AccountUser>();

        /// <summary>
        /// get - A collection of all the roles for this account.
        /// </summary>
        public ICollection<AccountRole> Roles { get; private set; } = new List<AccountRole>();

        /// <summary>
        /// get - A collection of calendars belonging to this account.
        /// </summary>
        public ICollection<Calendar> Calendars { get; private set; } = new List<Calendar>();

        /// <summary>
        /// get - A collection of schedules belonging to this account.
        /// </summary>
        public ICollection<Schedule> Schedules { get; private set; } = new List<Schedule>();

        /// <summary>
        /// get - A collection of questions that can be used for openings.
        /// </summary>
        public ICollection<Question> Questions { get; private set; } = new List<Question>();
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an Account object.
        /// </summary>
        public Account()
        {

        }

        /// <summary>
        /// Creates a new instance of an Account object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="kind"></param>
        /// <param name="subscription"></param>
        public Account(User owner, AccountKind kind, Subscription subscription)
        {
            this.OwnerId = owner?.Id ?? throw new ArgumentNullException(nameof(owner));
            this.Owner = owner;
            this.SubscriptionId = subscription?.Id ?? throw new ArgumentNullException(nameof(subscription));
            this.Subscription = subscription;
            this.Kind = kind;
            this.Key = Guid.NewGuid();
        }
        #endregion
    }
}
