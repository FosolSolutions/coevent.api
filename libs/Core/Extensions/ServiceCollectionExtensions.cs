using CoEvent.Core.Mvc.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoEvent.Core.Extensions
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods
        /// <summary>
        /// Configure the response headers middleware and add it to the services collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddResponseHeaders(this IServiceCollection services, Action<ResponseHeaderBuilder> options)
        {
            var builder = new ResponseHeaderBuilder();
            options?.Invoke(builder);
            var policy = builder.Build();
            return services.AddSingleton(policy);
        }
        #endregion
    }
}
