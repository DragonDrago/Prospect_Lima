namespace Lima.Dictionaries.Api.Domain
{
    public class DrugFilter
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public string ProducerName { get; set; }
        public int CountryId { get; set; }
        public int StorePlaceId { get; set; }
        public int ExpireDateMonth { get; set; }
        public bool HaveAndOrder { get; set; }
    }
}
