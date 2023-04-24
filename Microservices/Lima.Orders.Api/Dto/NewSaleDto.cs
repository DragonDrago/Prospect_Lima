using System.Collections.Generic;

namespace Lima.Orders.Api.Dto
{
    public class NewSaleDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; }
        public int PrepaymentPercent { get; set; }
        public List<SaleDetailingDto> SalesDetailing { get; set; }
    }
}
