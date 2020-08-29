using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace CoEvent.Core.Mvc.Filters
{
    /// <summary>
    /// ValidateModelFilterAttribute class, provides a filter to automatically return a bad request (HTTP 400) if the model state is invalid.
    /// </summary>
    public class ValidateModelFilterAttribute : ActionFilterAttribute
    {
        #region Methods
        /// <summary>
        /// If the ModelState is invalid then respond with the error messages.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // TODO: Should only return certain types of errors to the client.
                context.Result = new BadRequestObjectResult(context.ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }
        #endregion
    }
}
