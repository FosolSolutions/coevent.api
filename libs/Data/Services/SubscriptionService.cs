using CoEvent.Data.Interfaces;

namespace CoEvent.Data.Services
{
    /// <summary>
    /// SubscriptionService sealed class, provides a way to manage subscriptions in the datasource.
    /// </summary>
    public sealed class SubscriptionService : UpdatableService<Entities.Subscription, Models.Subscription>, ISubscriptionService
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a SubscriptionService object, and initalizes it with the specified options.
        /// </summary>
        /// <param name="source"></param>
        internal SubscriptionService(IDataSource source) : base(source)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the subscription for the specified 'id'.
        /// Validates whether the current user is authorized to view the subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Subscription Get(int id)
        {
            return this.Find(id);
        }
        #endregion
    }
}
