using BuildingBlocks.Messaging.MassTransit;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.StateMachines;

namespace Orchestration.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrchestrationDbConnection")
            ?? throw new InvalidOperationException("Connection string 'OrchestrationDbConnection' is missing.");

      

        services.AddDbContext<OrchestrationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IOrchestrationDbContext>(sp => sp.GetRequiredService<OrchestrationDbContext>());


        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<OrchestrationDbContext>();
                    r.UsePostgres();
                    r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                });

            x.AddEntityFrameworkOutbox<OrchestrationDbContext>(o =>
            {
                o.UsePostgres();
                o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
            });

            x.AddConfigureEndpointsCallback((context, _, cfg) =>
            {
                cfg.UseEntityFrameworkOutbox<OrchestrationDbContext>(context);
            });

            var asbConnection = configuration["MessageBroker:AzureServiceBusConnectionString"] ?? throw new InvalidOperationException("MessageBroker:AzureServiceBusConnectionString is missing.");

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(asbConnection);
                cfg.ConfigureEndpoints(context);
            });
            //x.UsingRabbitMq((context, cfg) =>
            //{
            //    cfg.Host(rabbitMqUri, h =>
            //    {
            //        h.Username(rabbitMqSettings.Username);
            //        h.Password(rabbitMqSettings.Password);
            //    });

            //    cfg.ConfigureEndpoints(context);
            //});
        });

        return services;
    }
}
