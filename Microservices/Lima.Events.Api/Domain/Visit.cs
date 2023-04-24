namespace Lima.Events.Api.Domain
{
    public class Visit
    {
        public int VisitId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
