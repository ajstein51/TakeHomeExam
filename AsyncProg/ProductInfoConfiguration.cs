using Microsoft.EntityFrameworkCore;

namespace AsyncProg
{
    public class ProductInfoConfiguration : DbContext
    {
        public DbSet<ProductInfo> ProductInfo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Normally put in appsettings.json
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;database=AsyncProg;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductInfo>().ToTable("Product");
        }
    }
}
