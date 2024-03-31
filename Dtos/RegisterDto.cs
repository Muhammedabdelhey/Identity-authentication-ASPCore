using System.ComponentModel.DataAnnotations;

namespace Identity_Authentication.Dtos
{
    public class RegisterDto
    {
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber {  get; set; }
    }
}
