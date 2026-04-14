using BuildingBlocks.Messaging.MassTransit;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Payment.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }

        public static IServiceCollection AddOrchestrationMessageBroker(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddMassTransit(config =>
            {

                config.SetKebabCaseEndpointNameFormatter();

           
                    config.AddConsumers(Assembly.GetExecutingAssembly());

                services.Configure<MessageBrokerSettings>(options =>
                {
                    options.Host = configuration["MessageBroker:Host"]!;
                    options.Username = configuration["MessageBroker:Username"]!;
                    options.Password = configuration["MessageBroker:Password"]!;
                });

                config.UsingRabbitMq((context, cfg) =>
                {
                    var messageBrokerSettings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                    cfg.Host(messageBrokerSettings.Host, h =>
                    {
                        h.Username(messageBrokerSettings.Username);
                        h.Password(messageBrokerSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
