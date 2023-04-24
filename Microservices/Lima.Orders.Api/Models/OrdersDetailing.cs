using System;
using System.Collections.Generic;

#nullable disable

namespace Lima.Orders.Api.Models
{
    public partial class OrdersDetailing
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int IncomeDetailingId { get; set; }
        public decimal? SalePrice { get; set; }
        public int? Amount { get; set; }
        public DateTime? DateCreate { get; set; }

        public virtual Order Order { get; set; }
    }
}
