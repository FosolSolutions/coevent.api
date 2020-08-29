using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// PrincipalExtensions static class, provides extension methods for IPrincipal.
    /// </summary>
    public static class PrincipalExtensions
    {
        #region Methods
        /// <summary>
        /// Get the current principal claim name identifier.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetNameIdentifier(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        }

        /// <summary>
        /// Get the current principal claim unique key.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetKey(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Key");
        }

        /// <summary>
        /// Get the current principal claim actual account, as they are currently impersonating another account.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetImpersonator(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Impersonator");
        }

        /// <summary>
        /// Get the current principal claim email address.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetEmail(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        }

        /// <summary>
        /// Get the current principal claim name.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetName(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        }

        /// <summary>
        /// Get the current principal claim surname.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetSurname(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
        }

        /// <summary>
        /// Get the current principal claim gender.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetGender(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender);
        }

        /// <summary>
        /// Get the current principal claim user.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetUser(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "User");
        }

        /// <summary>
        /// Get the current principal claim whether they are a participant account instead of a user account.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetParticipant(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Participant");
        }

        /// <summary>
        /// Get the current principal claim calendar.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetCalendar(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Calendar");
        }

        /// <summary>
        /// Get the current principal claim account.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Claim GetAccount(this IPrincipal principal)
        {
            return (principal as ClaimsPrincipal)?.Claims.FirstOrDefault(c => c.Type == "Account");
        }
        #endregion
    }
}
