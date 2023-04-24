using System.ComponentModel.DataAnnotations;

namespace Lima.AuthenticationServer.Api.ViewModel
{
    public class AuthenticationRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
