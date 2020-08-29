using System.ComponentModel.DataAnnotations;

namespace CoEvent.Api.Models.Auth
{
    /// <summary>
    /// LoginModel class, provides a model that represents login information.
    /// </summary>
    public class LoginModel
    {
        #region Properties
        /// <summary>
        /// get/set - The account username.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// get/set - The account password.
        /// </summary>
        [Required]
        public string Password { get; set; }
        #endregion
    }
}
