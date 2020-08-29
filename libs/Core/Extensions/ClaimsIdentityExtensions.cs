using System.Linq;
using System.Security.Claims;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// ClaimsIdentityExtensions static class, provides extension methods for ClaimIdentity.
    /// </summary>
    public static class ClaimsIdentityExtensions
    {
        #region Methods
        /// <summary>
        /// Get the name identifier claim.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Claim GetNameIdentifier(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        }

        public static Claim GetKey(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "Key");
        }

        public static Claim GetImpersonator(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "Impersonator");
        }

        public static Claim GetEmail(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        }

        public static Claim GetName(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        }

        public static Claim GetSurname(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
        }

        public static Claim GetGender(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender);
        }

        public static Claim GetParticipant(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "Participant");
        }

        /// <summary>
        /// Get the identity claim calendar.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Claim GetCalendar(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "Calendar");
        }

        /// <summary>
        /// Get the identity claim account.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Claim GetAccount(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "Account");
        }

        /// <summary>
        /// Get the identity claim user.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Claim GetUser(this ClaimsIdentity identity)
        {
            return identity.Claims.FirstOrDefault(c => c.Type == "User");
        }
        #endregion
    }
}
