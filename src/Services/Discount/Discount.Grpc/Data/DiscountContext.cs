using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{

    //DATA ACCESS LAYER
    public class DiscountContext:DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id = 1 , ProductName = "IPhone 13", Description = "IPhone 13 discounting", Amount = 50 },
                 new Coupon { Id = 2, ProductName = "Samsung Galaxy S21", Description = "Samsung Galaxy S21 discounting", Amount = 100 }
                );
        }
        public DbSet<Coupon> Coupons { get; set; } = default!;
    }
}
