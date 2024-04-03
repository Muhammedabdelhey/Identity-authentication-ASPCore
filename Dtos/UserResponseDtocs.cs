using System.IdentityModel.Tokens.Jwt;

namespace Identity_Authentication.Dtos
{
    public class UserResponseDtocs
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public  required TokenResponse Token { get; set; }
    }

}
