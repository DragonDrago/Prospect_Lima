namespace Lima.Sales.Api.Requests
{
    public class SalesDetailingRequest
    {
        public int DrugId { get; set; }
        public int Amount { get; set; }
        public decimal SalePrice { get; set; }
    }
}
