using Microsoft.EntityFrameworkCore;
using DSW_TP1.Presentacion.Controllers;
using DSW_TP1.Datos;
using DSW_TP1.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DSW_TP1.Presentacion.DTO;

namespace Test3.Products
{
    public class ProductsControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // base única por test
                .Options;

            var context = new AppDbContext(options);

            // Datos de ejemplo
            context.Products.AddRange(
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-001",
                    InternalCode = "INT-001",
                    Name = "Producto A",
                    Description = "Descripción A",
                    CurrentUnitPrice = 10,
                    StockQuantity = 100
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-002",
                    InternalCode = "INT-002",
                    Name = "Producto B",
                    Description = "Descripción B",
                    CurrentUnitPrice = 20,
                    StockQuantity = 200
                }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task GetProducts_ReturnsPagedProducts_WhenParametersAreValid()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = await controller.GetProducts(pageNumber: 1, pageSize: 10);

            var okResult = Assert.IsType<OkObjectResult>(result);

            // Convertimos el objeto anónimo a JSON y luego a nuestro DTO
            var json = JsonSerializer.Serialize(okResult.Value);
            var response = JsonSerializer.Deserialize<ProductDTO>(json)!;

            Assert.Equal(2, response.TotalItems);
            Assert.Equal(1, response.PageNumber);
            Assert.Equal(10, response.PageSize);

            Assert.Equal(2, response.Items.Count);
            Assert.Contains(response.Items, p => p.Name == "Producto A");
            Assert.Contains(response.Items, p => p.Name == "Producto B");
        }

        [Fact]
        public async Task GetProducts_ReturnsBadRequest_WhenParametersAreInvalid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            // Act
            var result = await controller.GetProducts(pageNumber: 0, pageSize: -1);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Los parámetros de paginación deben ser mayores que 0.", badRequest.Value);
        }
    }
}
