using AutoMapper;
using CoEvent.Data.Interfaces;
using CoEvent.Data.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;

namespace CoEvent.Data
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for service collections.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Variables
        private const string ConnectionStringName = "CoEventWeb";
        private const string DbUserId = "DB_USERID";
        private const string DbPassword = "DB_PASSWORD";
        #endregion

        #region Methods
        /// <summary>
        /// Add the CoEventtoria database context to the service collection and apply the default configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoEventContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var cs = configuration.GetConnectionString(ConnectionStringName);
            var builder = new SqlConnectionStringBuilder(cs);
            var user = configuration[DbUserId];
            var pwd = configuration[DbPassword];
            if (!String.IsNullOrEmpty(user))
            {
                builder.UserID = user;
            }
            if (!String.IsNullOrEmpty(pwd))
            {
                builder.Password = pwd;
            }
            services.AddDbContext<CoEventContext>(options =>
            {
                var sql = options.UseSqlServer(builder.ConnectionString, o =>
                {
                    o.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });

            return services;
        }

        /// <summary>
        /// Add the CoEventtoria database context to the service collection, apply the default configuration and allow for some customization to configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoEventContext(this IServiceCollection services, IConfiguration configuration, Action<DbContextOptionsBuilder> config)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (config == null) throw new ArgumentNullException(nameof(config));

            var cs = configuration.GetConnectionString(ConnectionStringName);
            var builder = new SqlConnectionStringBuilder(cs);
            var user = configuration[DbUserId];
            var pwd = configuration[DbPassword];
            if (!String.IsNullOrEmpty(user))
            {
                builder.UserID = user;
            }
            if (!String.IsNullOrEmpty(pwd))
            {
                builder.Password = pwd;
            }
            services.AddDbContext<CoEventContext>(o =>
            {
                var sql = o.UseSqlServer(builder.ConnectionString, o =>
                {
                    o.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
                config(sql);
            });

            return services;
        }

        /// <summary>
        /// Add the CoEventtoria database context to the service collection, requires manual configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddCoEventContext(this IServiceCollection services, Action<DbContextOptionsBuilder> config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            services.AddDbContext<CoEventContext>(o =>
            {
                config(o);
            });

            return services;
        }

        /// <summary>
        /// Adds the datasource to the service collection and configures the DbContext options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataSource(this IServiceCollection services, Action<DataSourceOptionsBuilder> setupAction)
        {
            var builder = new DataSourceOptionsBuilder();
            setupAction?.Invoke(builder);
            services.AddSingleton<DbContextOptions>(builder.Options);
            services.AddSingleton(builder.Options);
            var profile = new ModelProfile();
            var mapper = new MapperConfiguration(config =>
            {
                //profile.BindDataSource(this);
                config.AllowNullCollections = true;
                config.AddProfile((Profile)profile);
            }).CreateMapper();
            services.AddSingleton<ModelProfile>(profile);
            services.AddSingleton<IMapper>(mapper);
            services.AddScoped<IDataSource, DataSource>();
            return services;
        }



        /// <summary>
        /// Adds the datasource to the service collection and configures the DbContext options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="optionsBuilder"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataSource(this IServiceCollection services, IConfiguration configuration, Action<DataSourceOptionsBuilder> setupAction)
        {
            return services.AddDataSource(options =>
            {
                var connectionString = configuration.GetConnectionString("ApiData") ?? @"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0";
                var builder = new SqlConnectionStringBuilder(connectionString);
                var user = configuration[DbUserId];
                var pwd = configuration[DbPassword];
                if (!String.IsNullOrEmpty(user))
                {
                    builder.UserID = user;
                }
                if (!String.IsNullOrEmpty(pwd))
                {
                    builder.Password = pwd;
                }
                options.UseApplicationServiceProvider(services.BuildServiceProvider());
                options.UseSqlServer(builder.ConnectionString);
                //services.AddDataSourcePool(options =>
                //{
                //    options.UseSqlServer(builder.ConnectionString);
                //});
                //optionsBuilder.UseInMemoryDatabase("Schedule", options => { });
                setupAction?.Invoke(options);
            });
        }

        /// <summary>
        /// Adds the datasource to the service collection and configures the DbContext options within a DbContextPool.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setupAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataSourcePool(this IServiceCollection services, Action<DataSourceOptionsBuilder> setupAction)
        {
            services.AddSingleton<ModelProfile>();
            services.AddScoped<IDataSource, DataSource>();
            services.AddDbContextPool<CoEventContext>((dbBuilder) =>
            {
                var builder = new DataSourceOptionsBuilder(dbBuilder);
                setupAction?.Invoke(builder);
                services.AddSingleton<DbContextOptions>(builder.Options);
                services.AddSingleton(builder.Options);
            });

            return services;
        }
        #endregion
    }
}
