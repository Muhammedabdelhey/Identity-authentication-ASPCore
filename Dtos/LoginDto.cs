using System.ComponentModel.DataAnnotations;

namespace Identity_Authentication.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
