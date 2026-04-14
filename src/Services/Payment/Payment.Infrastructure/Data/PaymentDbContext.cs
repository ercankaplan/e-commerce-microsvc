using Microsoft.EntityFrameworkCore;
using Payment.Application.Data;
using Payment.Domain.Models;
using System.Reflection;

namespace Payment.Infrastructure.Data
{
    public class PaymentDbContext : DbContext, IPaymentDbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
