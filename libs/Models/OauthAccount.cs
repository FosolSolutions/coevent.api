using System;

namespace CoEvent.Models
{
    public class OauthAccount : BaseModel
    {
        #region Properties
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Issuer { get; set; }

        public string Key { get; set; }
        #endregion
    }
}
