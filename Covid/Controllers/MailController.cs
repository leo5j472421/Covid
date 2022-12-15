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
        public ApiResonse SendTestMail()
        {
            _mailService.sendTestMail();
            return new ApiResonse();
        }
    }

    public class ApiResonse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}