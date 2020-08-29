using CoEvent.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoEvent.Core.Mvc.Middleware
{
    public class JsonExceptionHandler
    {
        #region Properties
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        #endregion

        #region Constructors
        public JsonExceptionHandler(RequestDelegate next, ILogger logger = null)
        {
            _next = next;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;
            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";

            return context.HandleExceptionResponse(exception);
        }
        #endregion
    }
}
