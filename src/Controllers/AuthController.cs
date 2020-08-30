using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CoEvent.Core.Extensions;
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoEvent.Api.Models.Auth;
using CoEvent.Api.Helpers.Authentication;
using CoEvent.Api.Helpers;
using System.Linq;
using CoEvent.Core.Exceptions;

namespace CoEvent.Api.Controllers
{
    /// <summary>
    /// AuthController class, provides a way to authenticate a user.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        #region Variables
        private readonly ILogger _logger;
        private readonly IDataSource _dataSource;
        private readonly IAuthenticationHelper _auth;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of an AuthController object, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="auth"></param>
        /// <param name="dataSource"></param>
        /// <param name="logger"></param>
        public AuthController(IAuthenticationHelper auth, IDataSource dataSource, ILogger<AuthController> logger)
        {
            _auth = auth;
            _dataSource = dataSource;
            _logger = logger;
        }
        #endregion

        #region Endpoints
        /// <summary>
        /// Get the current identity of the logged in user.  This may be a particpant or a user depending on how they signed in.
        /// </summary>
        /// <returns></returns>
        [HttpGet("current/identity"), Authorize]
        public IActionResult CurrentPrincipal()
        {
            var participantId = User.GetParticipant()?.Value.ConvertTo<int>();
            var userId = User.GetUser()?.Value.ConvertTo<int>();

            if (userId.HasValue)
            {
                var user = _dataSource.Users.Get(userId.Value);
                return Ok(user);
            }
            else
            {
                var participant = _dataSource.Participants.Get(participantId.Value);
                return Ok(participant);
            }
        }

        /// <summary>
        /// Authenticate the user and return a JWT token.
        /// </summary>
        /// <returns></returns>
        [HttpPost("token/user")]
        public async Task<IActionResult> UserTokenAsync(LoginModel login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (String.IsNullOrWhiteSpace(login.Username)) throw new ArgumentException("User information is not valid.", nameof(login));

            var user = _auth.Validate(login.Username, login.Password);
            return new JsonResult(await _auth.AuthenticateAsync(user));
        }

        /// <summary>
        /// Validate the key and sign the participant in.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("token/participant")]
        public async Task<IActionResult> ParticipantToken(Guid key)
        {
            var participant = _auth.FindParticipant(key);
            return new JsonResult(await _auth.AuthenticateAsync(participant));
        }

        /// <summary>
        /// Refresh the access token if the refresh token is valid.
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var id = User.GetNameIdentifier()?.Value ?? throw new NotAuthenticatedException();
            var key = Guid.Parse(id);
            var access = User.Claims.FirstOrDefault(c => c.Type == "AccessType")?.Value ?? throw new NotAuthenticatedException();

            if (access == "User")
            {
                var user = _auth.FindUser(key);
                return new JsonResult(await _auth.AuthenticateAsync(user));
            }
            else if (access == "Participant")
            {
                var participant = _auth.FindParticipant(key);
                return new JsonResult(await _auth.AuthenticateAsync(participant));
            }

            throw new NotAuthenticatedException();
        }

        /// <summary>
        /// Login with a default user account.
        /// </summary>
        /// <returns></returns>
        [HttpPost("token/backdoor/user")]
        public async Task<IActionResult> BackdoorUser()
        {
            var user = _auth.FindUser("admin@fosol.ca"); // TODO: By configuration.
            if (user == null)
                return BadRequest();

            return new JsonResult(await _auth.AuthenticateAsync(user));
        }

        /// <summary>
        /// Signs the current user in as the specified participant.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost("impersonate/participant/{key}"), Authorize]
        public async Task<IActionResult> ImpersonateParticipant(Guid key)
        {
            // Participants are not allowed to impersonate.
            if (User.GetParticipant() != null) return Unauthorized();

            // TODO: Verify that the user is allowed to perform this action.
            var userId = User.GetUser().Value.ConvertTo<int>();
            var participant = _dataSource.Participants.Get(key);
            if (participant == null)
                return BadRequest();

            var identity = _dataSource.Participants.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating participant '{participant.Id}' - current user: '{userId}'.");

            var participantClaim = identity.GetParticipant();
            if (participantClaim != null)
            {
                identity.RemoveClaim(participantClaim);
            }

            var impersonateClaim = identity.GetImpersonator();
            if (impersonateClaim != null)
            {
                identity.RemoveClaim(impersonateClaim);
            }

            identity.AddClaim(new Claim("Participant", $"{participant.Id}", typeof(int).Name, "CoEvent"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", typeof(int).Name, "CoEvent")); // TODO: Issuer

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(participant);
        }

        /// <summary>
        /// Signs the current user in as the specified user.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("impersonate/user/{key}"), Authorize]
        public async Task<IActionResult> ImpersonateUser(Guid key)
        {
            // Participants are not allowed to impersonate.
            if (User.GetParticipant() != null) return Unauthorized();

            // TODO: Verify that the user is allowed to perform this action.
            var userId = User.GetUser().Value.ConvertTo<int>();
            var user = _dataSource.Users.Get(key);
            if (user == null)
                return BadRequest();

            var identity = _dataSource.Users.CreateIdentity(key);
            if (identity == null)
                return Unauthorized();

            _logger.LogInformation($"Impersonating user '{user.Id}' - current user: '{userId}'.");

            var identifierClaim = identity.GetNameIdentifier();
            if (identifierClaim != null)
            {
                identity.RemoveClaim(identifierClaim);
            }

            var userClaim = identity.GetUser();
            if (userClaim != null)
            {
                identity.RemoveClaim(userClaim);
            }

            var impersonateClaim = identity.GetImpersonator();
            if (impersonateClaim != null)
            {
                identity.RemoveClaim(impersonateClaim);
            }

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{user.Key}", typeof(Guid).Name, "CoEvent"));
            identity.AddClaim(new Claim("User", $"{user.Id}", typeof(int).Name, "CoEvent"));
            identity.AddClaim(new Claim("Impersonator", $"{userId}", typeof(int).Name, "CoEvent")); // TODO: Issuer

            var impersonate = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, impersonate);

            return Ok(user);
        }
        #endregion
    }
}
