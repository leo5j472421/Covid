using System.Collections.Generic;
using Covid.Controllers;

namespace Covid.Repositories.Interfaces
{
    public interface IMailRepository
    {
        IEnumerable<MailInfo> GetSentMailList(SendMailRequest request);
    }

    public class MailInfo
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        
    }
}