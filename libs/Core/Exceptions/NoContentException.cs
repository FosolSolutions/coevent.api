using System;

namespace CoEvent.Core.Exceptions
{
    /// <summary>
    /// NoContentException sealed class, provides a common implementation for handling requests for content that could not be found.
    /// </summary>
    public sealed class NoContentException : Exception
    {
        #region Constructors
        /// <summary>
        /// Creates new instance of a NoContentException object, and initializes it with the specified property.
        /// </summary>
        /// <param name="type"></param>
        public NoContentException(Type type) : base($"The {type.Name} could not be found in the datasource.")
        {

        }
        #endregion
    }
}
