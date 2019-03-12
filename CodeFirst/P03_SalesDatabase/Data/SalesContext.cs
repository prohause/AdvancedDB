using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateCustomerEntity(modelBuilder);
            CreateProductEntity(modelBuilder);
            CreateStoreEntity(modelBuilder);
            CreateSaleEntity(modelBuilder);
        }

        private static void CreateSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sale>()
                .HasKey(s => s.SaleId);

            modelBuilder
                .Entity<Sale>()
                .Property(s => s.Date)
                .HasDefaultValueSql("GETDATE()");
        }

        private static void CreateStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Store>()
                .HasKey(st => st.StoreId);

            modelBuilder
                .Entity<Store>()
                .HasMany(st => st.Sales)
                .WithOne(s => s.Store);

            modelBuilder
                .Entity<Store>()
                .Property(st => st.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();
        }

        private static void CreateProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Sales)
                .WithOne(s => s.Product);

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(250);
        }

        private static void CreateCustomerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Sales)
                .WithOne(s => s.Customer);

            modelBuilder
                .Entity<Customer>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(80)
                .IsRequired();

            modelBuilder
                .Entity<Customer>()
                .Property(c => c.CreditCardNumber)
                .IsRequired();
        }
    }
}