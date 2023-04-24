using System;
using System.Collections.Generic;

namespace Lima.Orders.Api.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Check { get; set; }
        public DateTime DateCreate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal OrderSum { get; set; }
        public decimal PrepaymentSum { get; set; }
        public int PrepaymentPercent { get; set; }
        public decimal RemainderSum { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public Organization Organization { get; set; }
        public Organization Distributor { get; set; }
        public List<OrderDetailings> OrderDetailings { get; set; }

        public Order()
        {
            OrderDetailings = new List<OrderDetailings>();
        }
    }
}
