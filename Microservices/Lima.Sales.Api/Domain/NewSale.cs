using System.Collections.Generic;

namespace Lima.Sales.Api.Domain
{
    public class NewSale
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int PrepaymentPercent { get; set; }
        public List<SaleDetailing> SalesDetailing { get; set; }
    }
}
