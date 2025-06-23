using DSW_TP1.Data;
using DSW_TP1.Models;

namespace DSW_TP1.Persistencia
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Si ya hay productos, no hacemos nada
            if (context.Products.Any()) return;

            var productos = new List<Product>
            {
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-001",
                    InternalCode = "PROD001",
                    Name = "Auriculares Bluetooth",
                    Description = "Auriculares inalámbricos con micrófono.",
                    CurrentUnitPrice = 25.000m,
                    StockQuantity = 5
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-002",
                    InternalCode = "PROD002",
                    Name = "Mouse Gamer",
                    Description = "Mouse óptico con luces LED.",
                    CurrentUnitPrice = 21500,
                    StockQuantity = 10
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-003",
                    InternalCode = "PROD003",
                    Name = "Teclado Mecánico",
                    Description = "Teclado retroiluminado RGB.",
                    CurrentUnitPrice = 30000,
                    StockQuantity = 8
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-004",
                    InternalCode = "PROD004",
                    Name = "Webcam HD",
                    Description = "Cámara web para videollamadas.",
                    CurrentUnitPrice = 23000,
                    StockQuantity = 10
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    SKU = "SKU-005",
                    InternalCode = "PROD005",
                    Name = "Cargador USB-C",
                    Description = "Cargador rápido de 25W.",
                    CurrentUnitPrice = 7500,
                    StockQuantity = 25
                }
            };

            context.Products.AddRange(productos);
            context.SaveChanges();
        }
    }
}
