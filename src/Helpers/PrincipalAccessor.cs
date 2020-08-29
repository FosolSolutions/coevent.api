using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace CoEvent.Api.Helpers
{
    /// <summary>
    /// PrincipalAccessor class, provides a way to get the currently logged in principal through DI.
    /// </summary>
    public class PrincipalAccessor : IPrincipalAccessor
    {
        #region Properties
        /// <summary>
        /// get - The current principal that is logged in.
        /// </summary>
        public ClaimsPrincipal Principal { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PrincipalAccessor object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="context"></param>
        public PrincipalAccessor(IHttpContextAccessor context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            this.Principal = context.HttpContext?.User;
        }
        #endregion
    }
}
