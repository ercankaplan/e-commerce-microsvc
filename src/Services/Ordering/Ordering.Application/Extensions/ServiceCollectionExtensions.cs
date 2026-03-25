using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                configuration.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.ValidationBehavior<,>));
                configuration.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.LoggingBehavior<,>));
            });

            return services;
        }
    }
}
