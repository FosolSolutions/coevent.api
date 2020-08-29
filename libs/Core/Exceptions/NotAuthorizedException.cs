using System;

namespace CoEvent.Core.Exceptions
{
    /// <summary>
    /// NotAuthorizedException sealed class, provides a common implementation for handling requests that are not authorized.
    /// </summary>
    public sealed class NotAuthorizedException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of a NotAuthorizedException object, and initializes it with the specified property.
        /// </summary>
        public NotAuthorizedException() : base($"The current user is not authorized to perform this action.")
        {

        }

        /// <summary>
        /// Creates new instance of a NotAuthorizedException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="message"></param>
        public NotAuthorizedException(string message) : base(message)
        {

        }
        #endregion
    }
}
