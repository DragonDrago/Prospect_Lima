namespace Lima.Events.Api.Dto
{
    public class NewVisitDto
    {
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int? DoctorPositionId { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorPhone { get; set; }
        public string Comment { get; set; }
        public int Prepayment { get; set; }
    }
}
