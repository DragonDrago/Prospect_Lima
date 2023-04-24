using System;

namespace Lima.Visits.Api.Domain
{
    public class VisitFilter
    {
        public int Offset { get; set; }
        public int MedrepId { get; set; }
        public string MedrepName { get; set; }
        public DateTime[] Dates { get; set; }
        public int StatusId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string DoctorName { get; set; }
        public int DoctorId { get; set; }
        public string Phone { get; set; }
        public string DoctorPosition { get; set; }
        public decimal[] Sales { get; set; }
        public decimal[] Prepayments { get; set; }
        public int UserId { get; set; }
    }
}
