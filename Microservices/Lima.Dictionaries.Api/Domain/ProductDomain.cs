using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lima.Dictionaries.Api.Domain
{
    public class ProductDomain
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public string Country { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasurements { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public string PhotoLink { get; set; }
    }
}
