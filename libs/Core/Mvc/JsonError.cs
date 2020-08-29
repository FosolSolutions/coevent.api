using CoEvent.Core.Exceptions;
using CoEvent.Core.Extensions;
using Newtonsoft.Json;
using System;
using System.Net;

namespace CoEvent.Core.Mvc
{
    /// <summary>
    /// JsonError class, provides a consistent generic way to handle and response in JSON error message information.
    /// </summary>
    public class JsonError
    {
        #region Properties
        /// <summary>
        /// get/set - The HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// get/set - The error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// get/set - The exception type.
        /// </summary>
        public string ExceptionType { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instatnce of a JsonError object.
        /// </summary>
        public JsonError()
        {
        }

        /// <summary>
        /// Creates a new instatnce of a JsonError object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public JsonError(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = (int)statusCode;
            this.Message = message;
        }

        /// <summary>
        /// Creates a new instatnce of a JsonError object, and initializes it with the specified properties.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        public JsonError(HttpStatusCode statusCode, Exception exception)
        {
            this.StatusCode = (int)statusCode;
            this.ExceptionType = exception?.GetType().Name;

            if (exception is NoContentException
                || exception is InvalidOperationException
                || exception is NotAuthenticatedException
                || exception is NotAuthorizedException)
            {
                this.Message = exception.GetAllMessages();
            }
            else
            {
                this.Message = "An unhandled error has occured.";
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Serializes this JsonError with default settings.  Use the JsonErrorHandler to use DI.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
