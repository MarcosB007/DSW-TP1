using Microsoft.EntityFrameworkCore;
using DSW_TP1.Datos;
using DSW_TP1.Dominio.Models;
using DSW_TP1.Presentacion.Controllers;
using DSW_TP1.Presentacion.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Test2.Orders
{
    public class OrdersControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // nombre único por test
                .Options;

            var context = new AppDbContext(options);

            // Producto de prueba
            context.Products.Add(new Product
            {
                ProductId = Guid.NewGuid(),
                SKU = "SKU123",
                InternalCode = "INT123",
                Name = "Producto Test",
                CurrentUnitPrice = 100m,
                StockQuantity = 10
            });

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedOrder_WhenDataIsValid()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var product = await context.Products.FirstAsync();

            var dto = new CreateOrderDto
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Av. Siempre Viva 123",
                BillingAddress = "Av. Siempre Viva 123",
                Notes = "Entrega urgente",
                OrderItems = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto
                {
                    ProductId = product.ProductId,
                    Quantity = 2,
                    UnitPrice = product.CurrentUnitPrice
                }
            }
            };

            // Act
            var result = await controller.CreateOrder(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var order = Assert.IsType<Order>(created.Value);

            Assert.Equal(dto.CustomerId, order.CustomerId);
            Assert.Single(order.OrderItems);
            Assert.Equal(200m, order.TotalAmount);
            Assert.Equal("Pending", order.OrderStatus);

            // Verifico que la orden esté en la base
            Assert.Equal(1, await context.Orders.CountAsync());
        }

    }
}

