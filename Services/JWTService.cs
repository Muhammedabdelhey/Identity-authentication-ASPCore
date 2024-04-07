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
        public async Task<JwtSecurityToken> GenreateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
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
            #region make token useing handel and descriptor
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.Lifetime)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = (JwtSecurityToken)token;
            #endregion
            #region make token with JwtSecurityToken
            //var accessToken = new JwtSecurityToken(
            //        issuer: _jwtOptions.Issuer,
            //        audience: _jwtOptions.Audience,
            //        signingCredentials: new SigningCredentials(
            //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)),
            //            SecurityAlgorithms.HmacSha256),
            //        claims: claims,
            //        expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Lifetime)
            //    );
            #endregion
            return accessToken;
        }
    }
}