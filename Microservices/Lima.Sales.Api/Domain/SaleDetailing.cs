namespace Lima.Sales.Api.Domain
{
    public class SaleDetailing
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public int Amount { get; set; }
        public decimal SalePrice { get; set; }
    }
}
