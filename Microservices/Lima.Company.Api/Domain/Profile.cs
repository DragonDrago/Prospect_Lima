using System;

namespace Lima.Company.Api.Domain
{
    public class Profile
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string RegionName { get; set; }
        public string Position { get; set; }
        public string RoleName { get; set; }
    }
}
