namespace Lima.Events.Api.Domain
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int OrganizationTypeId { get; set; }
        public string OrganizationTypeName { get; set; }
        public double OrganizationLatitude { get; set; }
        public double OrganizationLongitude { get; set; }
    }
}
