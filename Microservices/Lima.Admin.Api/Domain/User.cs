using System;

namespace Lima.Admin.Api.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime Birthday { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
