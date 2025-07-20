using Module_7_JwtAuth.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Module_7_JwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]

        public IActionResult Login([FromBody] UserModel user)
        {
            if (IsValidUser(user))
            {
                var token=GenerateJwtToken(user.Username);
                return Ok(new { token });
            }
            return Unauthorized("Invalid username or password.");
        }

        private bool IsValidUser(UserModel user)
        {
            // Replace with your user validation logic
            return user.Username == "admin" && user.Password == "password";
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
             new Claim(ClaimTypes.Name, username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
