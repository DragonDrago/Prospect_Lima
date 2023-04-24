using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lima.Dictionaries.Api.Domain
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Inn { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int? HealthCareFacilityTypeId { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
