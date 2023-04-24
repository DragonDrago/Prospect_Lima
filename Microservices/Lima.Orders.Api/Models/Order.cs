using System;
using System.Collections.Generic;

#nullable disable

namespace Lima.Orders.Api.Models
{
    public partial class Order
    {
        public Order()
        {
            OrdersDetailings = new HashSet<OrdersDetailing>();
        }

        public int Id { get; set; }
        public int? VisitId { get; set; }
        public int? StatusId { get; set; }
        public string Name { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? PrepaymentPercent { get; set; }

        public virtual Visit Visit { get; set; }
        public virtual ICollection<OrdersDetailing> OrdersDetailings { get; set; }
    }
}
