using Microsoft.EntityFrameworkCore;
using DSW_TP1.Dominio.Models;

namespace DSW_TP1.Datos
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Usuarios> Usuarios{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Order → OrderItems
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.order!)
                .HasForeignKey(oi => oi.OrderId);

            // Relación Product → OrderItems
            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product!)
                .HasForeignKey(oi => oi.ProductId);

            //// Insertamos un usuario al crear la base de datos
            modelBuilder.Entity<Usuarios>().HasData(
                new Usuarios
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "1234" //
                }
            );
        }
    }
}
