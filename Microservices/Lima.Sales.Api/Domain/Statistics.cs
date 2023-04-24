namespace Lima.Sales.Api.Domain
{
    public class Statistics
    {
        public int PendingCount { get; set; }
        public decimal PendingSum { get; set; }
        public int ShippedCount { get; set; }
        public decimal ShippedSum { get; set; }
    }
}
