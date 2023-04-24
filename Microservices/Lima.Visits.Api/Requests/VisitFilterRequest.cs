using Microsoft.AspNetCore.Mvc;
using System;

namespace Lima.Visits.Api.Requests
{
    public class VisitFilterRequest
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; }

        [FromQuery(Name = "medrep_id")]
        public int MedrepId { get; set; }

        [FromQuery(Name = "medrep_name")]
        public string MedrepName { get; set; }

        [FromQuery(Name = "dates")]
        public DateTime[] Dates { get; set; }

        [FromQuery(Name = "status")]
        public int StatusId { get; set; }

        [FromQuery(Name = "org_id")]
        public int OrganizationId { get; set; }

        [FromQuery(Name = "org_name")]
        public string OrganizationName { get; set; }

        [FromQuery(Name = "doc_name")]
        public string DoctorName { get; set; }

        [FromQuery(Name = "doc")]
        public int DoctorId { get; set; }

        [FromQuery(Name = "phone")]
        public string Phone { get; set; }

        [FromQuery(Name = "doc_pos")]
        public string DoctorPosition { get; set; }

        [FromQuery(Name = "sales")]
        public decimal[] Sales { get; set; }

        [FromQuery(Name = "prepayments")]
        public decimal[] Prepayments { get; set; }
    }
}
