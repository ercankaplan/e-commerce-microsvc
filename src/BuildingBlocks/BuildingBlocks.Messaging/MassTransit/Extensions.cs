using MassTransit;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration, Assembly? assembly = null)
        {
            services.AddMassTransit(config    =>
            {

                config.SetKebabCaseEndpointNameFormatter();

                if (assembly != null) // If an assembly is provided, register all consumers from that assembly => Ordering API
                    config.AddConsumers(assembly);

                //services.Configure<MessageBrokerSettings>(options =>
                //{
                //    options.Host = configuration["MessageBroker:Host"]!;
                //    options.Username = configuration["MessageBroker:Username"]!;
                //    options.Password = configuration["MessageBroker:Password"]!;
                //});

     

                var asbConnection = configuration["MessageBroker:AzureServiceBusConnectionString"] ?? throw new InvalidOperationException("MessageBroker:AzureServiceBusConnectionString is missing.");

                config.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(asbConnection);
                    cfg.ConfigureEndpoints(context);
                });

                //config.UsingRabbitMq((context, cfg) =>
                //{
                //    var messageBrokerSettings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                //    cfg.Host(messageBrokerSettings.Host, h =>
                //    {
                //        h.Username(messageBrokerSettings.Username);
                //        h.Password(messageBrokerSettings.Password);
                //    });

                //    cfg.ConfigureEndpoints(context);
                //});
            });
            return services;
        }
    }
}
