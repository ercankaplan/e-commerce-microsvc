using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Data;
using Payment.Application.Interfaces;
using Payment.Infrastructure.Data;
using Payment.Infrastructure.PaymentProviders.BankAnt;

namespace Payment.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PaymentDB")
                ?? throw new InvalidOperationException("Connection string 'PaymentDB' is missing.");

            services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions =>
                    sqlOptions.MigrationsAssembly(typeof(PaymentDbContext).Assembly.FullName)));

            services.AddScoped<IPaymentDbContext>(sp => sp.GetRequiredService<PaymentDbContext>());
            services.AddScoped<IPaymentProvider, BankAntVirtualPost>();

            return services;
        }
    }
}
