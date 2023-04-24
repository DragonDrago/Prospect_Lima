namespace Lima.Visits.Api.Domain
{
    public class DoctorStatistics
    {
        public int Compleated { get; set; }
        public int Planned { get; set; }
        public int Overdue { get; set; }
        public string MostTell { get; set; }
        public string TalkLess { get; set; }
    }
}
