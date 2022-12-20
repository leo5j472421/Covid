using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.Caching;
using Covid.Cache.Interface;
using Covid.Enums;
using Covid.Repositories;
using Covid.Repositories.Interfaces;

namespace Covid.Cache
{
    public class AttachmentCache : CacheBase<List<Attachment>>
    {
        private readonly IMailRepository _repo;

        public AttachmentCache(IMailRepository repo):base(EnumCache.Attachments)
        {
            _repo = repo;
        }

        protected override List<Attachment> ReloadFromDb(string key)
        {
            var dbResult = _repo.GetAttachments(int.Parse(key));
            var result = new List<Attachment>();
            foreach (var attachmentFromDb in dbResult)
            {
                var attachment = new Attachment(attachmentFromDb.Path);
                if (attachmentFromDb.isLinkImage)
                {
                    attachment.ContentId = attachmentFromDb.ContentId;
                }

                result.Add(attachment);
            }
            return result;
        }

        protected override CacheItemPolicy GetItemPolicy()
        {
            return new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };
        }
    }

    public class AttachmentFromDb
    {
        public string Path { get; set; }
        public bool isLinkImage { get; set; }
        public string ContentId { get; set; }
    }
}