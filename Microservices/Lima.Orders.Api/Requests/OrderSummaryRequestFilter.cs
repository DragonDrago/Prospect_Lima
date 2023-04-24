using Lima.Core;
using System;

namespace Lima.Orders.Api.Requests
{
    public class OrderSummaryRequestFilter
    {
        public int[] DrugsCount { get; set; }
        public string OrganizationName { get; set; }
        public string DoctorName { get; set; }
        public decimal[] TotalSum { get; set; }
    }
}
