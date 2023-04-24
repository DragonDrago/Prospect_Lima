using System.Collections.Generic;

namespace Lima.Orders.Api.Requests
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public decimal PrepaymentPercent { get; set; }
        public List<OrderDetailingRequest> OrdersDetailing { get; set; }
    }
}
