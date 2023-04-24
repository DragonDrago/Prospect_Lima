using Microsoft.AspNetCore.Mvc;

namespace Lima.Dictionaries.Api.Requests
{
    public class OrganizationFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "org_name")]
        public string OrganizationName { get; set; }

        [FromQuery(Name = "org_inn")]
        public string Inn { get; set; }

        [FromQuery(Name = "sales")]
        public int[] Sales { get; set; }

        [FromQuery(Name = "orders")]
        public int[] Orders { get; set; }

        [FromQuery(Name = "total_sum")]
        public decimal[] TotalSum { get; set; }

        [FromQuery(Name = "prepay")]
        public decimal[] Prepayment { get; set; }
    }
}
