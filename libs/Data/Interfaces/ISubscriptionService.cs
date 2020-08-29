namespace CoEvent.Data.Interfaces
{
    public interface ISubscriptionService : IUpdatableService<Models.Subscription>
    {
        /// <summary>
        /// Get the subscription for the specified 'id'.
        /// Validates whether the current user is authorized to view the subscription.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Subscription Get(int id);
    }
}
