using System;
using Microsoft.AspNetCore.Authentication;

namespace CoEvent.Core.Exceptions
{
    /// <summary>
    /// OauthException sealed class, provides a common implementation for handling oauth requests.
    /// </summary>
    public sealed class OauthException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of a OauthException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="exception"></param>
        public OauthException(Exception exception) : base("An error occured while attempting to authenticate with oauth provider.", exception)
        {

        }

        /// <summary>
        /// Creates new instance of a OauthException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public OauthException(string message, Exception innerException) : base(message, innerException)
        {

        }

        /// <summary>
        /// Creates new instance of a OauthException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="context"></param>
        public OauthException(RemoteFailureContext context) : this(context.Failure)
        {

        }
        #endregion
    }
}
