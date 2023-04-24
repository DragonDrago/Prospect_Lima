using System.ComponentModel.DataAnnotations;

namespace Lima.Dictionaries.Api.Requests
{
    public class OrganizationRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Укажите название организации.")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Inn { get; set; }
        public int? HealthCareFacilityTypeId { get; set; }
        public int TypeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Укажите регион.")]
        public int RegionId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
