using System.Collections.Generic;
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
            return QuerySP<MailInfo>(spName: "GetSentMailInfo", request);
        }

        public IEnumerable<AttachmentFromDb> GetAttachments(int id)
        {
            return QuerySP<AttachmentFromDb>(spName: "GetAttachments", new
            {
                mailTemplateId = id
            });
        }
    }
}