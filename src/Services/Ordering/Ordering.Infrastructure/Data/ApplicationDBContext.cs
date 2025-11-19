using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using System.Reflection;

namespace Ordering.Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Product> Products => Set<Product>();


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasConversion(
                          v => v.Value,
                          v => CustomerId.Of(v));
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasConversion(
                          v => v.Value,
                          v => OrderId.Of(v));
                entity.Property(e => e.CustomerId)
                      .HasConversion(
                          v => v.Value,
                          v => CustomerId.Of(v));
                entity.OwnsOne(e => e.Payment);
            });
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasConversion(
                          v => v.Value,
                          v => OrderItemId.Of(v));
                entity.Property(e => e.OrderId)
                      .HasConversion(
                          v => v.Value,
                          v => OrderId.Of(v));
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasConversion(
                          v => v.Value,
                          v => ProductId.Of(v));
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
            */

            //  IEntityTypeConfiguration<T>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
          

        }
    }
}
