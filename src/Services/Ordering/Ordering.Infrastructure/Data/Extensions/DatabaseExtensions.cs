using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Ordering.Infrastructure.Data.Extensions
{
    public static class DatabaseExtensions
    {
       
        public static async Task InitializeDatabaseAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await SeedCustomersAsync(dbContext);
            await SeedProductsAsync(dbContext);
            await SeedOrdersAsync(dbContext);
      

        }

        private static async Task SeedCustomersAsync(ApplicationDbContext dbContext)
        {
            if(await dbContext.Customers.AnyAsync()) return;

            // Implement your data seeding logic here
            dbContext.Customers.AddRange(InitialData.Customers);
                // Add your customers here
          
            await dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }


        private static async Task SeedProductsAsync(ApplicationDbContext dbContext)
        {
            if(await dbContext.Products.AnyAsync()) return;

            // Implement your data seeding logic here
            dbContext.Products.AddRange(InitialData.Products);
            await dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }
        private static async Task SeedOrdersAsync(ApplicationDbContext dbContext)
        {
            if (await dbContext.Orders.AnyAsync()) return;
            // Implement your data seeding logic here
            dbContext.Orders.AddRange(InitialData.OrdersWithItems(InitialData.Customers,InitialData.Products));

            // Add your orders here

            await dbContext.SaveChangesAsync();
            await Task.CompletedTask;
        }


    }
}
