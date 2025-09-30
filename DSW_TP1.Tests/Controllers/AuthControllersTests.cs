using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DSW_TP1.Presentacion.Controllers;
using DSW_TP1.Datos;
using DSW_TP1.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Test1.Login
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ← nombre único
                .Options;

            var context = new AppDbContext(options);

            context.Usuarios.Add(new Usuarios
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "1234"
            });

            context.SaveChanges();
            return context;
        }


        private IConfiguration GetFakeConfig()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "EstaEsUnaClaveSuperSeguraDeMasDe32Chars123!" },
                { "Jwt:Issuer", "https://localhost" },
                { "Jwt:Audience", "https://localhost" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task Login_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var config = GetFakeConfig();
            var controller = new AuthController(config, context);

            var loginRequest = new Usuarios
            {
                Username = "admin",
                PasswordHash = "1234"
            };

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Extraigo el token directamente del objeto anónimo
            var token = okResult.Value.GetType().GetProperty("token")?.GetValue(okResult.Value) as string;
            Assert.False(string.IsNullOrEmpty(token));

            // Verifico que el token sea un JWT válido
            var handler = new JwtSecurityTokenHandler();
            Assert.True(handler.CanReadToken(token));

            var jwtToken = handler.ReadJwtToken(token);
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name);
            Assert.Equal("admin", nameClaim?.Value);
        }


        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsInvalid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var config = GetFakeConfig();
            var controller = new AuthController(config, context);

            var loginRequest = new Usuarios
            {
                Username = "admin",
                PasswordHash = "wrongpassword"
            };

            // Act
            var result = await controller.Login(loginRequest);

            // Assert
            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Contraseña incorrecta", unauthorized.Value);
        }
    }
}
