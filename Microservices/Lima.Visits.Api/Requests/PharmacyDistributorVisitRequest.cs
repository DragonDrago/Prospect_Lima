namespace Lima.Visits.Api.Requests
{
    public class PharmacyDistributorVisitRequest
    {
        public int VisitId { get; set; }
        public int VisitTypeId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int PrepaymentPercent { get; set; }
        public DrugRequest[] Drugs { get; set; }
        public string Comment { get; set; }
    }
}
