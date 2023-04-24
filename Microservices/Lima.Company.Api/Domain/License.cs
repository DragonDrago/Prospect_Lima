using System;

namespace Lima.Company.Api.Domain
{
    public class License
    {
        public string CompanyName { get; set; }
        public string Inn { get; set; }
        public string LicenseCode { get; set; }
        public int EmployeeCount { get; set; }
        public DateTime LicenseDate { get; set; }
    }
}
