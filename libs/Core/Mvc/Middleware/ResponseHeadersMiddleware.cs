using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CoEvent.Core.Mvc.Middleware
{
    public class ResponseHeadersMiddleware
    {
        #region Variables
        private readonly RequestDelegate _next;
        private readonly ResponseHeaderPolicy _policy;
        #endregion

        #region Constructors
        public ResponseHeadersMiddleware(RequestDelegate next, ResponseHeaderPolicy policy)
        {
            _next = next;
            _policy = policy;
        }
        #endregion

        #region Methods
        public async Task Invoke(HttpContext context)
        {
            var headers = context.Response.Headers;

            foreach (var header in _policy.SetHeaders)
            {
                headers[header.Key] = header.Value;
            }

            foreach (var header in _policy.RemoveHeaders)
            {
                headers.Remove(header);
            }

            await _next(context);
        }
        #endregion
    }
}
