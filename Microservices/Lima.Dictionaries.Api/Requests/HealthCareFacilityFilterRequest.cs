using Microsoft.AspNetCore.Mvc;

namespace Lima.Dictionaries.Api.Requests
{
    public class HealthCareFacilityFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "org_id")]
        public int OrganizationId { get; set; }

        [FromQuery(Name = "org_name")]
        public string OrganizationName { get; set; }

        [FromQuery(Name = "inn")]
        public string Inn { get; set; }
    }
}
