using System.Collections.Generic;

namespace Lima.Dictionaries.Api.Domain
{
    public class HealthCareFacility : Organization
    {
        public List<Doctor> Doctors { get; set; }

        public HealthCareFacility()
        {
            Doctors = new List<Doctor>();
        }
    }
}
