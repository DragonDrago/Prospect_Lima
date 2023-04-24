using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lima.Dictionaries.Api.Domain
{
    public class DoctorOrganization
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
