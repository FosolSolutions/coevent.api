using CoEvent.Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text.Json;

namespace CoEvent.Core.Mvc
{
    public class JsonErrorHandler
    {
        #region Properties
        public JsonSerializerSettings Settings { get; }
        public JsonSerializerOptions Options { get; }
        public IHostingEnvironment Environment { get; }
        #endregion

        #region Constructors
        private JsonErrorHandler(IOptions<MvcJsonOptions> options, IHostingEnvironment environment)
        {
            this.Settings = options?.Value?.SerializerSettings ?? throw new ArgumentNullException(nameof(options));
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public JsonErrorHandler(IOptions<JsonSerializerOptions> options, IHostingEnvironment environment)
        {
            this.Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }
        #endregion

        #region Methods
        public JsonError Wrap(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            if (this.Environment.IsDevelopment())
            {
                return new JsonError(statusCode, exception.GetAllMessages());
            }
            else
            {
                return new JsonError(statusCode, exception);
            }
        }

        public string Serialize(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            var error = Wrap(exception, statusCode);
            if (this.Options != null)
                return System.Text.Json.JsonSerializer.Serialize(error, this.Options);

            if (this.Settings != null)
                return JsonConvert.SerializeObject(error, this.Settings);

            return System.Text.Json.JsonSerializer.Serialize(error);
        }
        #endregion
    }
}
