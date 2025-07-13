using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWebApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // This allows anonymous access to generate tokens
    public class AuthController : ControllerBase
    {
        [HttpGet("generate-token")]
        public IActionResult GenerateToken(int userId = 1, string userRole = "Admin")
        {
            try
            {
                var token = GenerateJSONWebToken(userId, userRole);
                return Ok(new { Token = token, Message = "Token generated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // In a real application, you would validate credentials against a database
            // For demo purposes, we'll use hardcoded values
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = GenerateJSONWebToken(1, "Admin");
                return Ok(new { Token = token, Message = "Login successful" });
            }
            else if (request.Username == "user" && request.Password == "password")
            {
                var token = GenerateJSONWebToken(2, "User");
                return Ok(new { Token = token, Message = "Login successful" });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }

        private string GenerateJSONWebToken(int userId, string userRole)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysuperdupersecret"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Name, $"User{userId}")
            };

            var token = new JwtSecurityToken(
                issuer: "mySystem",
                audience: "myUsers",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10), // Token expires in 10 minutes
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}