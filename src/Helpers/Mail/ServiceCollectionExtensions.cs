using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoEvent.Api.Helpers.Mail
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the MailClient to the service provider for dependency injection.
        /// Configure the MailClient with the specified options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddMailClient(this IServiceCollection services, IConfiguration configuration, Action<MailOptions> setupAction)
        {
            var options = new MailOptions();
            var config = configuration.GetSection("Mail");
            config.Bind(options);
            services.Configure<MailOptions>(config);
            setupAction?.Invoke(options);
            return services.AddSingleton<MailClient>();
        }

        /// <summary>
        /// Add the MailClient to the service provider for dependency injection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMailClient(this IServiceCollection services)
        {
            return services.AddSingleton<MailClient>();
        }
    }
}
