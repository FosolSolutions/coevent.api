using CoEvent.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CoEvent.Api.Controllers
{
    /// <summary>
    /// ApiController class, provides API endpoints that describe all other endpoints in this application.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        #region Methods
        /// <summary>
        /// Returns an array of all the API endpoints in this application with their documentation.
        /// </summary>
        /// <param name="name">The name of the controller.</param>
        /// <returns>An array of Endpoint.</returns>
        [HttpGet("endpoints/{name?}")]
        public IActionResult Endpoints(string name)
        {
            var endpoints = ApiHelper.GetEndpoints(name);
            return Ok(ApiHelper.ConvertToObject());
        }

        /// <summary>
        /// Return only the endpoint with the specified name.
        /// </summary>
        /// <param name="name">The name and or path to the endpoint.  This uses dot-notation (i.e. area.controller.endpoint).</param>
        /// <returns>An object containing the endpoint information.</returns>
        [HttpGet("endpoint/{name}")]
        public IActionResult Endpoint(string name)
        {
            var result = ApiHelper.GetEndpoint(name);
            return result != null ? Ok(result) : BadRequest();
        }

        /// <summary>
        /// Get the model definition of the specified type.
        /// </summary>
        /// <param name="name">The name of the mode type.</param>
        /// <returns>An object containing the model type definition.</returns>
        [HttpGet("model/{name}")]
        public IActionResult Model(string name)
        {
            var type = Assembly.GetAssembly(typeof(CoEvent.Models.Calendar)).GetType($"CoEvent.Models.{name}", false, true) ?? Assembly.GetAssembly(typeof(CoEvent.Data.Entities.Calendar)).GetType($"CoEvent.Data.Entities.{name}", false, true);

            if (type == null) return BadRequest();

            return Ok(ApiHelper.GetModel(type));
        }

        /// <summary>
        /// Get a model example of the specified type.
        /// </summary>
        /// <param name="name">The name of the mode type.</param>
        /// <returns>An object with default example values.</returns>
        [HttpGet("model/{name}/example")]
        public IActionResult ModelExample(string name)
        {
            var type = Assembly.GetAssembly(typeof(CoEvent.Models.Calendar)).GetType($"CoEvent.Models.{name}", false, true) ?? Assembly.GetAssembly(typeof(CoEvent.Data.Entities.Calendar)).GetType($"CoEvent.Data.Entities.{name}", false, true);

            if (type == null) return BadRequest();

            return Ok(ApiHelper.GetModelExample(type));
        }
        #endregion
    }
}
