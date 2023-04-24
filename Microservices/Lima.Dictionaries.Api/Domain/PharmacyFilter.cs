namespace Lima.Dictionaries.Api.Domain
{
    public class PharmacyFilter
    {
        public int Offset { get; set; }
        public int Page { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationType { get; set; }
        public string Inn { get; set; }
        public int[] Sales { get; set; }
        public int[] Orders { get; set; }
        public decimal[] TotalSum { get; set; }
        public decimal[] Prepayment { get; set; } 
    }
}
