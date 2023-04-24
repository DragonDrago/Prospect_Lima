using Microsoft.AspNetCore.Mvc;
using System;

namespace Lima.Sales.Api.Requests
{
    public class SalesFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "sale")]
        public int SaleId { get; set; }

        [FromQuery(Name = "dates")]
        public DateTime[] Dates { get; set; }

        [FromQuery(Name = "user")]
        public int UserId { get; set; }

        [FromQuery(Name = "org")]
        public int OrganizationId { get; set; }

        [FromQuery(Name = "sale_sum")]
        public decimal[] SaleSum { get; set; }
    }
}
