using Microsoft.AspNetCore.Mvc;

namespace Lima.Incomes.Api.Requests
{
    public class SummaryFilterRequest
    {
        [FromQuery(Name = "drug_name")]
        public string DrugName { get; set; }

        [FromQuery(Name = "producing_country")]
        public string ProducingCountry { get; set; }

        [FromQuery(Name = "producer_name")]
        public string ProducerName { get; set; }

        [FromQuery(Name = "exists_order")]
        public bool? ExistsOrder { get; set; }
    }
}
