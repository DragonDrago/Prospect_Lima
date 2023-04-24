using System;

namespace Lima.Tenant.Api.Domain
{
    public class TenantInfo
    {
        public string TenantId { get; set; }
        public string DbName { get; set; }
        public string ConnectionString { get; set; }
        public string CompanyName { get; set; }
        public DateTime LicenseDate { get; set; }
    }
}
