using Microsoft.EntityFrameworkCore;
using QueryEfficiency.Entities;

namespace QueryEfficiency.Configurations;

public class DBContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<CustomerPrice> CustomerPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Normally put in appsettings.json
        optionsBuilder.UseLazyLoadingProxies()
            .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;database=QueryEfficiency;Trusted_Connection=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("Products")
            .HasKey(p => new { p.Id, p.Number });
        modelBuilder.Entity<CartItem>().ToTable("CartItems")
            .HasKey(ci => ci.ProductNumber);
        modelBuilder.Entity<Order>().ToTable("Orders")
            .HasKey(o => o.Id);
        modelBuilder.Entity<OrderLine>().ToTable("OrderLine")
            .HasKey(ol => new { ol.OrderId, ol.ProductId });
        modelBuilder.Entity<CustomerPrice>().ToTable("CustomerPrices")
            .HasKey(cp => new { cp.Id, cp.ProductNumber });
    }
}
