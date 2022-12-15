using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Covid.Enums;
using Covid.Exceptions;
using Covid.Model;
using Covid.Services.Interfaces;
using ActionFilterAttribute = Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute;

namespace Covid.Filter
{
    public class WebExceptionFilter : ActionFilterAttribute
    {
        private readonly ILoggerService _loggerService;

        public WebExceptionFilter(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _loggerService.Error(
                    $"Api request exception : {context.Exception.Message} :: Stack {context.Exception}");
                var errorCode = context.Exception is ApiException exception ? exception.Error : EnumError.GeneralError;
                context.Result = new ObjectResult(new ApiBaseResponse<object>()
                {
                    ErrorCode = errorCode
                });
                context.ExceptionHandled = true;
            }
        }
    }
}