
using CoEvent.Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CoEvent.Data
{
    public static class ApplicationBuilderExtensions
    {
        #region Methods
        /// <summary>
        /// Initalizes the datasource.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDataSource(this IApplicationBuilder app, IHostingEnvironment env = null)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var datasource = scope.ServiceProvider.GetService<IDataSource>();
                //datasource.EnsureCreated();
                datasource.Migrate();
                //datasource.EnsureDeleted();
            }

            return app;
        }
        #endregion
    }
}
