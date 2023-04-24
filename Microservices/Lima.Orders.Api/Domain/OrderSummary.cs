namespace Lima.Orders.Api.Domain
{
    public class OrderSummary
    {
        public int OrderId { get;set; }
        public Organization Organization { get; set; }
        public int DrugsAmount { get; set; }
        public decimal TotalSum { get; set; }
        public Doctor Doctor { get; set; }
    }
}
