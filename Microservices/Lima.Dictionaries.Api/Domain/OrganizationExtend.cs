namespace Lima.Dictionaries.Api.Domain
{
    public class OrganizationExtend : Organization
    {
        public int SaleCount { get; set; }
        public int OrderCount { get; set; }
        public decimal SaleTotalSum { get; set; }
        public decimal PrepaymentSum { get; set; }
        public decimal OrderTotalSum { get; set; }
        public decimal DeptSum => OrderTotalSum - PrepaymentSum;
    }
}
