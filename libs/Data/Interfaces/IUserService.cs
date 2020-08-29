using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace CoEvent.Data.Interfaces
{
    public interface IUserService : IUpdatableService<Models.User>
    {
        /// <summary>
        /// Verify the user with the specified key or email exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        int Verify(string email);

        /// <summary>
        /// Get the calendar for the specified 'id'.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Models.User Get(int id);

        /// <summary>
        /// Get the calendar for the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Models.User Get(Guid key);

        /// <summary>
        /// Get the claimed identity of the user for the specified 'userId'.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Claim> GetClaims(int userId);
    }
}
