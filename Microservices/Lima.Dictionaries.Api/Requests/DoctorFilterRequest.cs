using Microsoft.AspNetCore.Mvc;

namespace Lima.Dictionaries.Api.Requests
{
    public class DoctorFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "phone")]
        public string Phone { get; set; }

        [FromQuery(Name = "doctor_id")]
        public int DoctorId { get; set; }

        [FromQuery(Name = "doctor_name")]
        public string DoctorName { get; set; }

        [FromQuery(Name = "organization_id")]
        public int OrganizationId { get; set; }
    }
}
