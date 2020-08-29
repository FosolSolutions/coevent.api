using System;

namespace CoEvent.Api.Models.Auth
{
    /// <summary>
    /// TokenModel class, provides a model to represent a successful authentication.
    /// </summary>
    public class TokenModel
    {
        #region Properties

        /// <summary>
        /// get/set - The JWT token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// get/set - Number of seconds until the access token expires.
        /// </summary>
        public double ExpiresIn { get; set; }

        /// <summary>
        /// get/set - The JWT refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// get/set - Number of seconds until the refresh token expires.
        /// </summary>
        public double RefreshExpiresIn { get; set; }

        /// <summary>
        /// get/set - The scope of the token request.
        /// </summary>
        public string Scope { get; set; }
        #endregion

        #region Constructors
        public TokenModel() { }

        public TokenModel(string accessToken, TimeSpan expiresIn, string refreshToken, TimeSpan refreshExpiresIn, string scope)
        {
            this.AccessToken = accessToken;
            this.ExpiresIn = expiresIn.TotalSeconds;
            this.RefreshToken = refreshToken;
            this.RefreshExpiresIn = refreshExpiresIn.TotalSeconds;
            this.Scope = scope;
        }
        #endregion
    }
}
