using Microsoft.AspNetCore.Mvc;

namespace Lima.Sales.Api.Requests
{
    public class PrepaymentFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "org")]
        public int OrganizationId { get; set; }

        [FromQuery(Name = "check_list")]
        public int[] DrugCount { get; set; }

        [FromQuery(Name = "sale")]
        public int SaleId { get; set; }

        [FromQuery(Name = "sale_sum")]
        public decimal[] SaleSum { get; set; }

        [FromQuery(Name = "prepay")]
        public decimal[] Prepayments { get; set; }
    }
}
