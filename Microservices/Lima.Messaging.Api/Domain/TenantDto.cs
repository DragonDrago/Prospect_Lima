﻿using System;

namespace Lima.Messaging.Api.Dto
{
    public class TenantDto
    {
        public string TenantId { get; set; }
        public string DbName { get; set; }
        public string CompanyName { get; set; }
        public string ConnectionString { get; set; }
        public DateTime LicenseDate { get; set; }
    }
}
