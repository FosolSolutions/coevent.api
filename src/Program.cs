using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoEvent.Api
{
    /// <summary>
    /// Program class, provides the main program starting point for the API.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The primary entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args);
            builder.Build().Run();
        }

        /// <summary>
        /// Create a default configuration and setup for a web application.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env != "VS")
                DotNetEnv.Env.Load();
            else
                DotNetEnv.Env.Load(".vs.env");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"connectionstrings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("mailsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"mailsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseUrls(config.GetValue<string>("ASPNETCORE_URLS"))
                .UseStartup<Startup>();
        }
    }
}
