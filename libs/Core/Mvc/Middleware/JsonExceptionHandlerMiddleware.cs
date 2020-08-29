using CoEvent.Core.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoEvent.Core.Mvc.Middleware
{
    public class JsonExceptionHandlerMiddleware
    {
        #region Properties
        private readonly RequestDelegate _next;
        #endregion

        #region Constructors
        public JsonExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
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
