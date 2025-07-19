using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projeto_Nutri.Application.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto_Nutri.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO login)
        {
            // Aqui você pode validar em um banco real. Por enquanto, aceitamos qualquer nome.
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Role))
            {
                return BadRequest(new { error = "Usuário ou papel inválido." });
            }

            // Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, login.Role)
            };

            // Configuração do token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                role = login.Role,
                username = login.Username
            });
        }
    }
}
