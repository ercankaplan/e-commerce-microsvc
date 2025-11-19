using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data.Interceptors;


namespace Ordering.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastuctureServices(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("OrderingDB");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDBContext>((serviceProvider,options) =>
            {
                options.AddInterceptors(serviceProvider.GetRequiredService<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDBContext).Assembly.FullName));
            });

            //services.AddScoped<IApplicationDBContext, ApplicationDBContext>();

            return services;
        }
    }
}
