using CoEvent.Core.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CoEvent.Core.Mvc
{
    [ApiController]
    [JsonExceptionFilter]
    public abstract class ApiController : ControllerBase
    {
    }
}
