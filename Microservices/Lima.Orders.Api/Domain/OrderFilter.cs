using System;

namespace Lima.Orders.Api.Domain
{
    public class OrderFilter
    {
        public int Offset { get; set; }
        public int OrderId { get; set; }
        public DateTime[] Dates { get; set; }
        public int UserId { get; set; }
        public int OrganizationTypeId { get; set; }
        public decimal[] Sum { get; set; }
    }
}
