using System;

namespace Lima.Sales.Api.Domain
{
    public class SalesFilter
    {
        public int Offset { get; set; }
        public int SaleId { get; set; }
        public DateTime[] Dates { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public decimal[] SaleSum { get; set; }
    }
}
