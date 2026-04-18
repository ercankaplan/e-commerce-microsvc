using BuildingBlocks.Messaging.MassTransit;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Payment.Application.Interfaces;
using Payment.Application.Services;
using System.Reflection;

namespace Payment.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
