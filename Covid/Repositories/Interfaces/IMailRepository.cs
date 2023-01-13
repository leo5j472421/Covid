using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using Covid.Cache;
using Covid.Controllers;
using Newtonsoft.Json.Linq;

namespace Covid.Repositories.Interfaces
{
    public interface IMailRepository
    {
        IEnumerable<MailInfo> GetSentMailList(SendMailRequest request);
        IEnumerable<AttachmentFromDb> GetAttachments(int parse);
        MailTemplate GetMailTemplate(int mailGroup);
        void SentMail(string email, int mailGroup);
    }

    public class MailTemplate
    {
        public string TemplateId { get; set; }
        public string Subject { get; set; }
        public string Body{ get; set; }
    }

    public class MailInfo
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }

    }
}