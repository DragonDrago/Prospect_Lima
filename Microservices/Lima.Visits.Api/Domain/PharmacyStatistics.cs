namespace Lima.Visits.Api.Domain
{
    public class PharmacyStatistics
    {
        public int Compleated { get; set; }
        public int Planned { get; set; }
        public int Overdue { get; set; }
        public decimal Orders { get; set; }
        public decimal Prepayments { get; set; }
        public decimal Remains { get; set; }
    }
}
