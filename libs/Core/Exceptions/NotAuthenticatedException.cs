using System;

namespace CoEvent.Core.Exceptions
{
    /// <summary>
    /// NotAuthenticatedException sealed class, provides a common implementation for handling requests that are not authenticated.
    /// </summary>
    public sealed class NotAuthenticatedException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of a NotAuthenticatedException object, and initializes it with the specified property.
        /// </summary>
        public NotAuthenticatedException() : base($"The request was not authenticated.")
        {

        }

        /// <summary>
        /// Creates new instance of a NotAuthenticatedException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="message"></param>
        public NotAuthenticatedException(string message) : base(message)
        {

        }
        #endregion
    }
}
