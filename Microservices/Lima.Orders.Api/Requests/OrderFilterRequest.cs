using Microsoft.AspNetCore.Mvc;
using System;

namespace Lima.Orders.Api.Requests
{
    public class OrderFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "order_id")]
        public int OrderId { get; set; }

        [FromQuery(Name = "date")]
        public DateTime[] Dates { get; set; }

        [FromQuery(Name = "user_id")]
        public int UserId { get; set; }

        [FromQuery(Name = "org_type")]
        public int OrganizationTypeId { get; set; }

        [FromQuery(Name = "sum")]
        public decimal[] Sum { get; set; }
    }
}
