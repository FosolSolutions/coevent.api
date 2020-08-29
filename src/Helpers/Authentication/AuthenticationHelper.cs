using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoEvent.Api.Models.Auth;
using CoEvent.Data;
using CoEvent.Data.Entities;
using CoEvent.Data.Interfaces;

namespace CoEvent.Api.Helpers.Authentication
{
    public class AuthenticationHelper : IAuthenticationHelper
    {
        #region Variables
        private readonly CoEventAuthenticationOptions _options;
        private readonly byte[] _salt;
        private readonly CoEventContext _context;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IDataSource _dataSource;
        #endregion

        #region Constructors
        public AuthenticationHelper(IOptions<CoEventAuthenticationOptions> options, CoEventContext context, IDataSource dataSource, IHttpContextAccessor httpContext)
        {
            _options = options.Value;
            _salt = Encoding.UTF8.GetBytes(_options.Salt);
            _context = context;
            _httpContext = httpContext;
            _dataSource = dataSource;
        }
        #endregion

        #region Methods
        public string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: _salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
        }

        public User FindUser(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username) ?? throw new InvalidOperationException("Unable to authenticate user.");
        }

        public User FindUser(Guid key)
        {
            return _context.Users.FirstOrDefault(u => u.Key == key) ?? throw new InvalidOperationException("Unable to authenticate user.");
        }

        public Participant FindParticipant(Guid key)
        {
            return _context.Participants.FirstOrDefault(u => u.Key == key) ?? throw new InvalidOperationException("Unable to authenticate participant.");
        }

        public User Validate(string username, string password)
        {
            var user = FindUser(username);
            var hash = HashPassword(password);
            if (user.Password != hash) throw new InvalidOperationException("Unable to authenticate user.");

            return user;
        }

        public async Task<TokenModel> AuthenticateAsync(User user)
        {
            var identity = _dataSource.Users.CreateIdentity(user.Key);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Key}"),
                new Claim("AccessType", "User", typeof(string).FullName, "coevent")
            };
            return await AuthenticationAsync(identity, claims);
        }

        public async Task<TokenModel> AuthenticateAsync(Participant participant)
        {
            var identity = _dataSource.Participants.CreateIdentity(participant.Key);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, $"{participant.Key}"),
                new Claim("AccessType", "Participant", typeof(string).FullName, "coevent")
            };
            return await AuthenticationAsync(identity, claims);
        }

        private async Task<TokenModel> AuthenticationAsync(ClaimsIdentity identity, params Claim[] refreshClaims)
        {
            var accessToken = GenerateJwtToken(new ClaimsPrincipal(identity), _options.AccessTokenExpiresIn);
            var refreshToken = GenerateJwtToken(GeneratePrincipal(JwtBearerDefaults.AuthenticationScheme, refreshClaims), _options.RefreshTokenExpiresIn);

            return await Task.FromResult(new TokenModel(accessToken, _options.AccessTokenExpiresIn, refreshToken, _options.RefreshTokenExpiresIn, _options.DefaultScope));
        }

        private ClaimsPrincipal GeneratePrincipal(string authenticationScheme, params Claim[] claims)
        {
            var identity = new ClaimsIdentity(claims, authenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        private string GenerateJwtToken(ClaimsPrincipal user, TimeSpan expiresIn)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Subject = user.Identity as ClaimsIdentity,
                Expires = DateTime.UtcNow.Add(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
