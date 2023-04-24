namespace Lima.Sales.Api.Domain
{
    public class PrepaymentFilter
    {
        public int Offset { get; set; }
        public int OrganizationId { get; set; }
        public int[] DrugCount { get; set; }
        public int SaleId { get; set; }
        public decimal[] SaleSum { get; set; }
        public decimal[] Prepayments { get; set; }
    }
}
