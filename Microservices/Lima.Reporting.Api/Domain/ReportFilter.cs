using System;

namespace Lima.Reporting.Api.Domain
{
    public class ReportFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
    }
}
