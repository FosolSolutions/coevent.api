using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CoEvent.Api.Models.Auth;
using CoEvent.Data.Entities;

namespace CoEvent.Api.Helpers.Authentication
{
    public interface IAuthenticationHelper
    {
        string HashPassword(string password);

        User FindUser(string username);
        User FindUser(Guid key);
        Participant FindParticipant(Guid key);

        User Validate(string username, string password);

        Task<TokenModel> AuthenticateAsync(User user);

        Task<TokenModel> AuthenticateAsync(Participant participant);
    }
}
