using CoEvent.Core.Exceptions;
using CoEvent.Core.Extensions;
using CoEvent.Core.Mvc;
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoEvent.Api.Helpers
{
    /// <summary>
    /// AuthenticationExtensions static class, provides helper methods for authentication.
    /// </summary>
    public static class AuthenticationExtensions
    {
        #region Methods
        // TODO: Replace with better implementation, use built-in error handling.
        /// <summary>
        /// When an oath authorization or token request fails.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task HandleOnRemoteFailure(this RemoteFailureContext context)
        {
            var handler = context.HttpContext.RequestServices.GetRequiredService<JsonErrorHandler>();

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(handler.Serialize(new OauthException(context.Failure)));
            context.HandleResponse();
        }

        /// <summary>
        /// When an oauth token has been successfully received.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task HandleOnTicketReceived(this TicketReceivedContext context)
        {
            await Task.Run(() =>
            {
                var datasource = context.HttpContext.RequestServices.GetRequiredService<IDataSource>();
                AuthenticationExtensions.AuthorizeOauthUser(datasource, context.Principal);
            });
            context.Success();
        }

        /// <summary>
        /// When an unauthenticated user is redirect to the signin page it will instead just return a 401.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Task HandleOnRedirectToLogin(this RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates an identity object for a participant that matches the specified 'key'.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ClaimsIdentity CreateIdentity(this IParticipantService service, Guid key)
        {
            if (key == Guid.Empty)
                return null;

            var participant = service.Get(key);
            var claims = service.GetClaims(participant.Id.Value);
            return new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Creates an identity object for a participant that matches the specified 'key'.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ClaimsIdentity CreateIdentity(this IUserService service, Guid key)
        {
            if (key == Guid.Empty)
                return null;

            var user = service.Get(key);
            var claims = service.GetClaims(user.Id.Value);
            return new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// When a user selects a calendar it must update the principal claims.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="principal"></param>
        /// <param name="calendarId"></param>
        public static void SelectCalendar(this ICalendarService service, ClaimsPrincipal principal, int calendarId)
        {
            var claims = service.GetClaims(calendarId);
            var identity = principal.Identity as ClaimsIdentity;
            claims.ForEach(c =>
            {
                var claim = identity.Claims.FirstOrDefault(cl => cl.Type == c.Type);
                if (claim != null) identity?.TryRemoveClaim(claim);
            });
            identity?.AddClaims(claims);
        }

        /// <summary>
        /// Authorize the user which was authenticated with Oauth.
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="principal"></param>
        public static void AuthorizeOauthUser(this IDataSource datasource, ClaimsPrincipal principal)
        {
            // TODO: Allow a user to have multiple Oauth accounts from different providers that link to the internal user by it's email account(s).
            // TODO: Redirect user to create a User Profile page.
            var email = principal.GetEmail()?.Value;
            var userId = datasource.Users.Verify(email);
            CoEvent.Models.User user;
            if (userId == 0)
            {
                user = new CoEvent.Models.User()
                {
                    Key = Guid.NewGuid(),
                    LastName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                    FirstName = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                    Email = email,
                    OauthAccounts = new List<CoEvent.Models.OauthAccount>(new[]
                    {
                        new CoEvent.Models.OauthAccount()
                        {
                            Key = principal.GetNameIdentifier()?.Value,
                            Email = email,
                            Issuer = principal.GetNameIdentifier().Issuer
                        }
                    })
                };
                datasource.Users.Add(user);
                datasource.CommitTransaction();
            }
            else
            {
                user = datasource.Users.Get(userId);
            }

            var identity = principal.Identity as ClaimsIdentity;
            var claims = new List<Claim>(new[]
            {
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}", typeof(string).FullName, "CoEvent"), // TODO: Issuer namespace.
                new Claim("User", $"{user.Id}", typeof(int).FullName, "CoEvent"),
                new Claim("Account", $"{user.DefaultAccountId ?? user.OwnedAccounts.FirstOrDefault()?.Id ?? 0}", typeof(int).FullName, "CoEvent")
            });
            claims.ForEach(c =>
            {
                var claim = identity.Claims.FirstOrDefault(cl => cl.Type == c.Type);
                if (claim != null) identity?.TryRemoveClaim(claim);
            });
            identity?.AddClaims(claims);
        }
        #endregion
    }
}
