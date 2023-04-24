namespace Lima.Orders.Api.Domain
{
    public class OrderDetailings
    {
        public int Id { get; set; }
        public decimal SalePrice { get; set; }
        public int Amount { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
    }
}
