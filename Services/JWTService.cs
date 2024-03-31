using Identity_Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity_Authentication.Services
{
    public class JWTService(JwtOptions _jwtOptions, UserManager<User> _userManager)
    {
        public async Task<string> GenreateToken(User user )
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var roles =await  _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.NameId ,user.Id),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.GivenName, user.UserName),
            };
            // need to add roles on cliams 
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String));
            }
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Convert token to string
            var accessToken = tokenHandler.WriteToken(token);
            return accessToken;
        }
    }
}
