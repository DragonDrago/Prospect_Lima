using System;

namespace Lima.Dictionaries.Api.Domain
{
    public class Drug
    {
        public int Id { get; set; }
        public string DrugName { get; set; }
        public int ProducerId { get; set; }
        public string ProducerName { get; set; } 
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public decimal BasePrice { get; set; }
        public int? StorePlaceId { get; set; }
        public string StorePlaceName { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int Amount { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public int DrugInOrderCount { get; set; }
        public string PhotoName { get; set; }
    }
}
