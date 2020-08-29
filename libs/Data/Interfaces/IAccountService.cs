namespace CoEvent.Data.Interfaces
{
    public interface IAccountService : IUpdatableService<Models.Account>
    {
        /// <summary>
        /// Get the account for the specified 'id'.
        /// Validates whether the current user is authorized to view the account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.Account Get(int id);
    }
}
