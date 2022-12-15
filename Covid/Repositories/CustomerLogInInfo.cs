using System;

namespace Covid.Repositories
{
    public class CustomerLogInInfo
    {
        public long Id { get; set; }
        public Guid Token { get; set; }
        public string SessionId { get; set; }
        public int FpId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Currency { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
        public bool Enabled { get; set; }
        public string HostName { get; set; }
    }
}                                               