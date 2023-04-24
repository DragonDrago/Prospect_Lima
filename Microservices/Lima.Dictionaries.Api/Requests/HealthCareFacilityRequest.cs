using System.Collections.Generic;

namespace Lima.Dictionaries.Api.Requests
{
    public class HealthCareFacilityRequest
    {
        public OrganizationRequest Organization { get; set; }
        public List<int> DoctorsIds { get; set; }
    }
}
