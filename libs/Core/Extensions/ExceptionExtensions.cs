using System;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// ExceptionExtensions static class, provides extension methods for Exception objects.
    /// </summary>
    public static class ExceptionExtensions
    {
        #region Methods
        /// <summary>
        /// Get all inner error messages
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetAllMessages(this Exception ex)
        {
            return $"{ex.Message} {ex.InnerException?.GetAllMessages()}";
        }
        #endregion
    }
}
