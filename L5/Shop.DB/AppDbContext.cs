using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Shop.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacja Category -> Product (1:*)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Product -> Stock (1:1)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Product>(p => p.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja Order -> OrderProduct (*:*)
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId }); // Klucz złożony

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Opcjonalnie: Można skonfigurować dodatkowe właściwości, takie jak długości pól czy inne ograniczenia
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }



    }
}
