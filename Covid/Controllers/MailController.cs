using Microsoft.AspNetCore.Mvc;
using Covid.Filter;
using Covid.Services.Interfaces;

namespace Covid.Controllers
{
    [Route("Mail")]
    [ServiceFilter(typeof(RequestLogFilter))]
    [ServiceFilter(typeof(WebExceptionFilter))]
    public class MailController : ControllerBase
    {
    }
}