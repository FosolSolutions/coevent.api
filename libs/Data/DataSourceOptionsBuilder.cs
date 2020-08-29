using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;

namespace CoEvent.Data
{
    /// <summary>
    /// DataSourceOptionsBuilder class, provides a way to configure the DataSource.
    /// </summary>
    public class DataSourceOptionsBuilder
    {
        #region Properties
        /// <summary>
        /// get - The configuration options for the DbContext.
        /// </summary>
        internal DbContextOptions<CoEventContext> Options { get { return (DbContextOptions<CoEventContext>)this.DbContextOptionsBuilder.Options; } }

        /// <summary>
        /// get - The builder for the DbContext options.
        /// </summary>
        public DbContextOptionsBuilder DbContextOptionsBuilder { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a DataSourceOptionsBuilder object.
        /// </summary>
        public DataSourceOptionsBuilder()
        {
            this.DbContextOptionsBuilder = new DbContextOptionsBuilder(new DbContextOptions<CoEventContext>());
        }

        /// <summary>
        /// Creates a new instance of a DataSourceOptionsBuilder object, and initializes it with the specified options.
        /// </summary>
        /// <param name="dbOptionsBuilder"></param>
        public DataSourceOptionsBuilder(DbContextOptionsBuilder dbOptionsBuilder)
        {
            this.DbContextOptionsBuilder = dbOptionsBuilder;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Build the DbContext configuration options.
        /// </summary>
        /// <param name="builder"></param>
        public void Build(DbContextOptionsBuilder builder)
        {

        }

        /// <summary>
        /// This is required to allow the DataSource to pull the currently logged in user.
        /// </summary>
        /// <typeparam name="TPrincipalAccessor"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public DataSourceOptionsBuilder AddPrincipalAccessor<TPrincipalAccessor>(IServiceCollection services)
            where TPrincipalAccessor : class, Interfaces.IPrincipalAccessor
        {
            services.AddScoped<Interfaces.IPrincipalAccessor, TPrincipalAccessor>();

            return this;
        }

        public DataSourceOptionsBuilder UseApplicationServiceProvider(IServiceProvider serviceProvider)
        {
            this.DbContextOptionsBuilder.UseApplicationServiceProvider(serviceProvider);

            return this;
        }

        public DataSourceOptionsBuilder UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            this.DbContextOptionsBuilder.UseLoggerFactory(loggerFactory);

            return this;
        }

        public DataSourceOptionsBuilder UseSqlServer(string connectionString)
        {
            this.DbContextOptionsBuilder.UseSqlServer(connectionString);

            return this;
        }

        public DataSourceOptionsBuilder UseSqlServer(DbConnection connection)
        {
            this.DbContextOptionsBuilder.UseSqlServer(connection);

            return this;
        }

        public DataSourceOptionsBuilder EnableSensitiveDataLogging()
        {
            this.DbContextOptionsBuilder.EnableSensitiveDataLogging();

            return this;
        }
        #endregion
    }
}
