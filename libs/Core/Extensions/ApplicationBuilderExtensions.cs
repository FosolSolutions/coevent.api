using CoEvent.Core.Exceptions;
using CoEvent.Core.Mvc;
using CoEvent.Core.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CoEvent.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        #region Methods
        /// <summary>
        /// Using the default 'UseExceptionHandler' all unhandled exceptions will be returned as JSON.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseJsonExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var status = HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = (int)status;

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionFeature != null)
                    {
                        await context.HandleExceptionResponse(exceptionFeature.Error);
                    }
                });
            });
        }

        /// <summary>
        /// Add the JsonException middleware to return all unhandled exceptions as JSON.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseJsonExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JsonExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Generic implementation for converting unhandled exceptions into JSON.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        internal static async Task HandleExceptionResponse(this HttpContext context, Exception exception)
        {
            var status = HttpStatusCode.InternalServerError;
            var logger = context.RequestServices.GetService<ILoggerFactory>()?.CreateLogger(exception.GetType().Name);
            var handler = context.RequestServices.GetRequiredService<JsonErrorHandler>();
            logger?.LogError(exception, $"An error occured while executing {context.Request.Path}.");

            // Some exceptions are expected and should return their error message.
            if (exception is InvalidOperationException || exception is NoContentException)
            {
                status = HttpStatusCode.BadRequest;
            }
            else if (exception is NotAuthenticatedException)
            {
                status = HttpStatusCode.Unauthorized;
            }
            else if (exception is NotAuthorizedException)
            {
                status = HttpStatusCode.Forbidden;
            }

            var error = handler.Serialize(exception, status);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            await context.Response.WriteAsync(error);
        }

        /// <summary>
        /// Add the ResponseHeaders middleware to include the configured response headers with each response.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app)
        {
            var policy = app.ApplicationServices.GetService<ResponseHeaderPolicy>() ?? new ResponseHeaderBuilder(builder => builder.AddDefaultSecurePolicy()).Build();
            return app.UseMiddleware<ResponseHeadersMiddleware>(policy);
        }

        /// <summary>
        /// Add the ResponseHeaders middleware to include the configured response headers with each response.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app, ResponseHeaderBuilder builder)
        {
            var policy = builder.Build();
            return app.UseMiddleware<ResponseHeadersMiddleware>(policy);
        }

        /// <summary>
        /// Add the ResponseHeaders middleware to include the configured response headers with each response.
        /// </summary>
        /// <param name="app"></param>
        public static IApplicationBuilder UseResponseHeaders(this IApplicationBuilder app, Action<ResponseHeaderBuilder> options)
        {
            var builder = new ResponseHeaderBuilder();
            options?.Invoke(builder);
            var policy = builder.Build();
            return app.UseMiddleware<ResponseHeadersMiddleware>(policy);
        }
        #endregion
    }
}
