using Microsoft.AspNetCore.Mvc;

namespace Lima.Visits.Api.Requests
{
    public class NewVisitRequest
    {
        public int VisitId { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int? DoctorPositionId { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorPresent { get; set; }
        public string Comment { get; set; }
        public int[] TalkedAboutTheDrugsIds { get; set; }
        public int Prepayment { get; set; }
        //public int ReturnPeriod { get; set; }
        public DrugRequest[] Drugs { get; set; }
    }
}
