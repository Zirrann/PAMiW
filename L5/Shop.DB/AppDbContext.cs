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
            base.OnModelCreating(modelBuilder);

            // Konfiguracja dla Product i Stock
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Product>(p => p.StockId);

            // Konfiguracja autoinkrementacji dla ID
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();  // Ustawia autoinkrementację dla ID

            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryId)
                .ValueGeneratedOnAdd();  // Autoinkrementacja dla CategoryId

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderId)
                .ValueGeneratedOnAdd();  // Autoinkrementacja dla OrderId

            modelBuilder.Entity<Stock>()
                .Property(s => s.StockId)
                .ValueGeneratedOnAdd();  // Autoinkrementacja dla StockId

            // Konfiguracja dla OrderProduct (relacja wiele do wielu)
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);
        }


    }
}
