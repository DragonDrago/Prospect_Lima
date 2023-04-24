namespace Lima.Visits.Api.Domain
{
    public class Visit
    {
        public int VisitId { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int? DoctorPositionId { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorPhone { get; set; }
        public string DoctorPresent { get; set; }
        public string Comment { get; set; }
        public int Prepayment { get; set; }
        public int ReturnPeriod { get; set; }
        public int[] TalkedAboutTheDrugsIds { get; set; }
        public SelectedDrug[] Drugs { get; set; }
    }
}
