using System.Collections.Generic;

namespace Lima.Orders.Api.Domain
{
    public class NewOrder
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public decimal PrepaymentPercent { get; set; }
        public List<NewOrderDetailing> OrdersDetailing { get; set; }
    }
}
