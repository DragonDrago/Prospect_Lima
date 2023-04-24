using System.Collections.Generic;

namespace Lima.AuthenticationServer.Api.Model
{
    public class User
    {
        public int Id { get;set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyUId { get; set; }
        public string TenantId { get; set; }
        public bool Active { get; set; }
        public List<Role> Roles { get; set; }
        public List<string> Grants { get; set; }

        public User()
        {
            Roles = new List<Role>();
            Grants = new List<string>();
        }
    }
}
