using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace CoEvent.Api.Helpers.Swagger
{
    /// <summary>
    /// SwaggerOptions class, the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class SwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public SwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "CoEvent", // TODO: From configuration
                Version = description.ApiVersion.ToString(),
                Description = "CoEvent RESTful API", // TODO: From configuration
                Contact = new OpenApiContact() { Name = "Support", Email = "support@fosol.ca" }, // TODO: From configuration
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://github.com/FosolSolutions/coevent.api/blob/master/LICENSE") } // TODO: From configuration
            };

            if (description.IsDeprecated)
            {
                info.Description += " (This API version has been deprecated).";
            }

            return info;
        }
    }
}
