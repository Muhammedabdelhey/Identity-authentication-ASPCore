using System.ComponentModel.DataAnnotations;

namespace Identity_Authentication.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
