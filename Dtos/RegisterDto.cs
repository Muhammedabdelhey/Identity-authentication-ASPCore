using System.ComponentModel.DataAnnotations;

namespace Identity_Authentication.Dtos
{
    public class RegisterDto
    {

        public required string Username { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber {  get; set; }
    }
}
