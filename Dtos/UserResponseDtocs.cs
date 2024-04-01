using System.IdentityModel.Tokens.Jwt;

namespace Identity_Authentication.Dtos
{
    public class UserResponseDtocs
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public TokenResponse Token { get; set; }
    }

}
