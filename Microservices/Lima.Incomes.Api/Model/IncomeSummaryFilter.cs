namespace Lima.Incomes.Api.Model
{
    public class IncomeSummaryFilter
    {
        public string DrugName { get; set; }
        public string ProducingCountry { get; set; }
        public string ProducerName { get; set; }
        public bool? ExistsOrder { get; set; }
    }
}
