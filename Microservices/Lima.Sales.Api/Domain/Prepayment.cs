namespace Lima.Sales.Api.Domain
{
    public class Prepayment
    {
        public int SaleId { get; set; }
        public decimal SaleTotal { get; set; }
        public int PrepaymentPercent { get; set; }
        public decimal PrepaymentSum => SaleTotal / 100 * PrepaymentPercent;
        public int DrugsCount { get; set; }
        public Organization Organization { get; set; }
    }
}
