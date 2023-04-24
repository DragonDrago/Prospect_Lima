namespace Lima.Dictionaries.Api.Domain
{
    public class HealthCareFacilityFilter
    {
        public int Page { get; set; }
        public int Offset { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Inn { get; set; }
    }
}
