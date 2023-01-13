using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using Covid.Cache;
using Covid.Controllers;
using Covid.Enums;
using Covid.Repositories.Interfaces;
using Covid.Services.Interfaces;

namespace Covid.Repositories
{
    public class MailRepository : RepositoryBase,IMailRepository
    {
        public MailRepository(IDbConnectionFactory dbConnectionFactory, ILoggerService loggerService) : base(dbConnectionFactory, loggerService, EnumDbConnection.Mail)
        {
        }

        public IEnumerable<MailInfo> GetSentMailList(SendMailRequest request)
        {
            return QuerySP<MailInfo>(spName: "GetSentMailInfo", new
            {
                isTest = request.IsTest,
                count = request.Count
            });
        }

        public IEnumerable<AttachmentFromDb> GetAttachments(int id)
        {
            return QuerySP<AttachmentFromDb>(spName: "GetAttachments", new
            {
                mailTemplateId = id
            });
        }

        public MailTemplate GetMailTemplate(int mailGroup)
        {
            return QuerySP<MailTemplate>(spName: "GetEmailTemplate", new
            {
                mailGroup = mailGroup
            }).FirstOrDefault();
        }

        public void SentMail(string email, int mailGroup)
        {
            QuerySP<DbResponse>(spName: "SentMail", new
            {
                mail = email,
                mailGroup = mailGroup
            });
        }
    }

    public class DbResponse
    {
        public int ErrorCode { get; set; }
    }
}