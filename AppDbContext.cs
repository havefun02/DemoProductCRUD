using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DemoCRUD
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);
                entity.Property(p => p.ProductName)
                               .IsRequired()
                               .HasMaxLength(100);
                entity.Property(p => p.Price)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

                entity.Property(p => p.StockQuantity)
                 .IsRequired();

            });

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
                var configSection = configBuilder.GetSection("ConnectionStrings");
                var connectionString = configSection["DefaultConnection"];
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0)));
            }
        }
    }

}
