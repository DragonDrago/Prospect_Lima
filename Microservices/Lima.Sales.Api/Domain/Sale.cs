using System;
using System.Collections.Generic;

namespace Lima.Sales.Api.Domain
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal SaleTotal { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string OrderCheck { get; set; }
        public User User { get; set; }
        public Organization Organization { get; set; }
    }
}
