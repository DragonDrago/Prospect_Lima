namespace Lima.Visits.Api.Domain
{
    public class SelectedDrug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public decimal BasePrice { get; set; }
        public int Package { get; set; }
    }
}
