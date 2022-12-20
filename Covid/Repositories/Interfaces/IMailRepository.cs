using System.Collections.Generic;
using System.Data;
using Covid.Cache;
using Covid.Controllers;

namespace Covid.Repositories.Interfaces
{
    public interface IMailRepository
    {
        IEnumerable<MailInfo> GetSentMailList(SendMailRequest request);
        IEnumerable<AttachmentFromDb> GetAttachments(int parse);
    }

    public class MailInfo
    {
        public int TemplateId { get; set; }
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        
    }
}