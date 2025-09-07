using Microsoft.EntityFrameworkCore;
using DSW_TP1.Dominio.Models;

namespace DSW_TP1.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

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
        }
    }
}
