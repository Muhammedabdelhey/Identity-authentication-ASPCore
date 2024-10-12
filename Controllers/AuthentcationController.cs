using Identity_Authentication.Dtos;
using Identity_Authentication.Models;
using Identity_Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Identity_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentcationController(UserManager<User> _userManager, JWTService _jwtService, SignInManager<User> _signInManager) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Regsiter([FromForm] RegisterDto userDto)
        {
            // valid user data
            if (!ModelState.IsValid) return BadRequest(ModelState);
            User user = new()
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
            };
            // create user 
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                var role = await _userManager.AddToRoleAsync(user, "Admin");
                if (role.Succeeded)
                {
                    var token = await _jwtService.GenerateToken(user);
                    return Ok(new UserResponseDtocs
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        Email = user.Email,
                        Phone = user.PhoneNumber,
                        Token = new TokenResponse
                        {
                            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                            ExpireOn = token.ValidTo
                        }
                    });
                }
            }
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            User? user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return BadRequest(new { message = "Invalid username or password" });
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                var token = await _jwtService.GenerateToken(user);
                return Ok(new UserResponseDtocs
                {
                    UserId = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    Token = new TokenResponse
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpireOn = token.ValidTo
                    }
                });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }
    }
}
