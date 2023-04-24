namespace Lima.Orders.Api.Domain
{
    public class NewOrderDetailing
    {
        public int DrugId { get; set; }
        public int Amount { get; set; }
        public decimal SalePrice { get; set; }
    }
}
