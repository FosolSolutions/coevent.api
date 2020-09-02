using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using CoEvent.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using CoEvent.Api.Helpers.Authentication;
using CoEvent.Api.Helpers.Mail;
using CoEvent.Api.Helpers.Middleware;
using CoEvent.Api.Helpers;
using CoEvent.Core.Mvc;
using System.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CoEvent.Api
{
    /// <summary>
    /// Startup class, provides a way to start, initialize and configure the API.
    /// </summary>
    public class Startup
    {
        #region Variables
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private const string AllowedOrigins = "allowedOrigins";
        #endregion

        #region Properties
        /// <summary>
        /// get - The application configuration settings.
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// get/set - The environment settings for the application.
        /// </summary>
        /// <value></value>
        public IWebHostEnvironment Environment { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instances of a Startup class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            this.Configuration = configuration;
            this.Environment = env;
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Startup>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configure dependency injection.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHttpContextAccessor();
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.IgnoreNullValues = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:IgnoreNullValues"]) ? Boolean.Parse(this.Configuration["Serialization:Json:IgnoreNullValues"]) : false;
                options.PropertyNameCaseInsensitive = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:PropertyNameCaseInsensitive"]) ? Boolean.Parse(this.Configuration["Serialization:Json:PropertyNameCaseInsensitive"]) : false;
                options.PropertyNamingPolicy = this.Configuration["Serialization:Json:PropertyNamingPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null;
                options.WriteIndented = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:WriteIndented"]) ? Boolean.Parse(this.Configuration["Serialization:Json:WriteIndented"]) : false;
                //options.Converters.Add(new JsonStringEnumConverter());
                //options.Converters.Add(new Int32ToStringJsonConverter());
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:IgnoreNullValues"]) ? Boolean.Parse(this.Configuration["Serialization:Json:IgnoreNullValues"]) : false;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:PropertyNameCaseInsensitive"]) ? Boolean.Parse(this.Configuration["Serialization:Json:PropertyNameCaseInsensitive"]) : false;
                    options.JsonSerializerOptions.PropertyNamingPolicy = this.Configuration["Serialization:Json:PropertyNamingPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null;
                    options.JsonSerializerOptions.WriteIndented = !String.IsNullOrWhiteSpace(this.Configuration["Serialization:Json:WriteIndented"]) ? Boolean.Parse(this.Configuration["Serialization:Json:WriteIndented"]) : false;
                    //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    //options.JsonSerializerOptions.Converters.Add(new Int32ToStringJsonConverter());
                });
            services.Configure<CoEventAuthenticationOptions>(this.Configuration.GetSection("Authentication"));
            services.Configure<MailOptions>(this.Configuration.GetSection("Mail"));
            services.AddCoEventContext(this.Configuration, options =>
            {
                if (!this.Environment.IsProduction())
                {
                    options.UseLoggerFactory(_loggerFactory);
                    options.EnableSensitiveDataLogging();
                }
            });
            services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();
            services.AddScoped<IMailHelper, MailHelper>();
            services.AddMailClient();

            services.AddDataSource(this.Configuration, optionsBuilder =>
            {
                optionsBuilder.AddPrincipalAccessor<PrincipalAccessor>(services);
                if (this.Environment.IsDevelopment())
                {
                    optionsBuilder.EnableSensitiveDataLogging();
                }
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            });

            services.AddCors(options =>
            {
                var withOrigins = this.Configuration.GetSection("Cors:WithOrigins").Value?.Split(" ") ?? new string[0];
                if (withOrigins.Any())
                {
                    options.AddPolicy(name: AllowedOrigins,
                        builder =>
                        {
                            builder
                                .WithOrigins(withOrigins)
                                .AllowAnyHeader()
                                .AllowAnyMethod(); ;
                        });
                }
            });

            var config = this.Configuration.GetSection("Authentication").Get<CoEventAuthenticationOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.Issuer,
                        ValidAudience = config.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
                        ClockSkew = TimeSpan.Zero
                    };
                    //options.Events = new JwtBearerEvents()
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        context.Token = context.Request.Cookies[config.Cookie.Name];
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });

            if (!this.Environment.IsProduction())
            {
                // Ignore invalid SSL certificates.
                services.AddHttpClient("HttpRequestClient")
                    .ConfigureHttpClient(config =>
                    {

                    })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    new HttpClientHandler()
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, errors) =>
                        {
                            return HttpClientHandler.DangerousAcceptAnyServerCertificateValidator(sender, cert, chain, errors);
                        }
                    });
            }

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // TODO: Add to configuration
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue; // TODO: Add to configuration
                options.MultipartBodyLengthLimit = int.MaxValue; // TODO: Add to configuration
            });
            services.AddSingleton<JsonErrorHandler>();


            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                // options.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;

            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Helpers.Swagger.SwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations(true);
                options.CustomSchemaIds(o => o.FullName);
                options.OperationFilter<Helpers.Swagger.SwaggerDefaultValues>();
                options.DocumentFilter<Helpers.Swagger.SwaggerDocumentFilter>();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Configure the application components.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(options =>
            {
                options.RouteTemplate = this.Configuration.GetValue<string>("Swagger:RouteTemplate");
            });
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(String.Format(this.Configuration.GetValue<string>("Swagger:EndpointPath"), description.GroupName), description.GroupName);
                }
                options.RoutePrefix = this.Configuration.GetValue<string>("Swagger:RoutePrefix");
            });

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseDataSource();

            app.UseRouting();
            app.UseCors(AllowedOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            _logger.LogInformation("Application has started");
        }
        #endregion
    }
}
