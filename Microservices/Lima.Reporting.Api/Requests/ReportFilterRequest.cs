using System;

namespace Lima.Reporting.Api.Requests
{
    public class ReportFilterRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BranchId { get; set; }
        public int UserId { get; set; }
    }
}
