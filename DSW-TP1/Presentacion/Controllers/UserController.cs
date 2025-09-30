using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DSW_TP1.Dominio.Models;
using DSW_TP1.Datos;
using Microsoft.EntityFrameworkCore;

namespace DSW_TP1.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Usuarios request)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) return Unauthorized("Usuario no encontrado");

            if (user.PasswordHash != request.PasswordHash)
                return Unauthorized("Contraseña incorrecta");

            var token = GenerateToken(user.Username);
            return Ok(new { token });
        }

        private string GenerateToken(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
