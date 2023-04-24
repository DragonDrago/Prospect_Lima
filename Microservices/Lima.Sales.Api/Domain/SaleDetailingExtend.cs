using System.Collections.Generic;

namespace Lima.Sales.Api.Domain
{
    public class SaleDetailingExtend
    {
        public List<SaleDetailing> SaleDetailing { get; set; }
        public List<OrderDetailing> OrderDetailing { get; set; }

        public SaleDetailingExtend()
        {
            SaleDetailing = new List<SaleDetailing>();
            OrderDetailing = new List<OrderDetailing>();
        }
    }
}
