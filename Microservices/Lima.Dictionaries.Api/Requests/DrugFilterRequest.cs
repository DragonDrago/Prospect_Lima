using Microsoft.AspNetCore.Mvc;

namespace Lima.Dictionaries.Api.Requests
{
    public class DrugFilterRequest
    {
        [FromQuery(Name = "drug_id")]
        public int DrugId { get; set; }

        [FromQuery(Name = "drug_name")]
        public string DrugName { get; set; }

        [FromQuery(Name = "producer_name")]
        public string ProducerName { get; set; }

        [FromQuery(Name = "country_id")]
        public int CountryId { get; set; }

        [FromQuery(Name = "store_place_id")]
        public int StorePlaceId { get; set; }

        [FromQuery(Name = "exp_date_month")]
        public int ExpireDateMonth { get; set; }

        [FromQuery(Name = "have_an_order")]
        public bool HaveAnOrder { get; set; }
    }
}
