using DSW_TP1.Datos;
using DSW_TP1.Dominio.Models;

namespace DSW_TP1.Datos.Persistencia
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            // Aseguramos que la base está creada y migrada
            context.Database.EnsureCreated();

            // 1️⃣ Insertamos usuario admin si no existe
            if (!context.Usuarios.Any())
            {
                var admin = new Usuarios
                {
                    Username = "admin",
                    PasswordHash = "1234" // ⚠️ Texto plano por ahora, después lo cambiamos a hash
                };

                context.Usuarios.Add(admin);
                context.SaveChanges();
            }

            // 2️⃣ Insertamos productos si no existen
            if (!context.Products.Any())
            {
                var productos = new List<Product>
                {
                    new Product
                    {
                        ProductId = Guid.NewGuid(),
                        SKU = "SKU-001",
                        InternalCode = "PROD001",
                        Name = "Auriculares Bluetooth",
                        Description = "Auriculares inalámbricos con micrófono.",
                        CurrentUnitPrice = 25000m,
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
}
