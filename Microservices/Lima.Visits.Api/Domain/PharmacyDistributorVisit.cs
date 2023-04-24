namespace Lima.Visits.Api.Domain
{
    public class PharmacyDistributorVisit
    {
        public int VisitId { get; set; }
        public string VisitTypeId { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public int PrepaymentPercent { get; set; }
        public SelectedDrug[] Drugs { get; set; }
        public string Comment { get; set; }
    }
}
