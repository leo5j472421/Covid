using System;
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
        private IMailService _mailService;

        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost]
        [Route("SentTestMail")]
        public ApiResponse<string> SendTestMail()
        {
            _mailService.sendTestMail();
            return new ApiResponse<string>();
        }

        [HttpPost]
        [Route("SendMail")]
        public ApiResponse<string> SendMail([FromBody] SendMailRequest request)
        {
            _mailService.SendMail(request);
            return new ApiResponse<string>();
        }
    }

    public class SendMailRequest
    {
        public int MailGroup { get; set; }
        public bool IsTest { get; set; }
    }

    public class ApiResponse<T>
    {
        public T Result { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}