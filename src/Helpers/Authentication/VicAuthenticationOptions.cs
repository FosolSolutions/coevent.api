using System;

namespace CoEvent.Api.Helpers.Authentication
{
    public class CoEventAuthenticationOptions
    {
        #region Properties
        public string Salt { get; set; }
        public string Issuer { get; set; }

        public string Audience { get; set; }
        public string Secret { get; set; }
        public CookieOptions Cookie { get; set; }
        public TimeSpan AccessTokenExpiresIn { get; set; } = new TimeSpan(0, 1, 0);
        public TimeSpan RefreshTokenExpiresIn { get; set; } = new TimeSpan(1, 0, 0);
        public string DefaultScope { get; set; } = "profile";
        #endregion
    }
}
