using System;

namespace Lima.Dictionaries.Api.Domain
{
    public class DrugSeries
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string FullName { get; set; }
        public string Series { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
