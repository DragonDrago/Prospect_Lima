using Microsoft.AspNetCore.Mvc;

namespace Lima.Dictionaries.Api.Requests
{
    public class DistributorFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "org_name")]
        public string OrganizationName { get; set; }
    }
}
