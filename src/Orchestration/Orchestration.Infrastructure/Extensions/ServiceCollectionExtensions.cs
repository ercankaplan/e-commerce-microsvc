using BuildingBlocks.Messaging.MassTransit;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.StateMachines;

namespace Orchestration.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrchestrationDbConnection")
            ?? throw new InvalidOperationException("Connection string 'OrchestrationDbConnection' is missing.");

        var rabbitMqSettings = new MessageBrokerSettings
        {
            Host = configuration["MessageBroker:Host"] ?? string.Empty,
            Username = configuration["MessageBroker:Username"] ?? string.Empty,
            Password = configuration["MessageBroker:Password"] ?? string.Empty,
            VirtualHost = configuration["MessageBroker:VirtualHost"] ?? "/",
            Port = ushort.TryParse(configuration["MessageBroker:Port"], out var port) ? port : (ushort)5672
        };

        if (string.IsNullOrWhiteSpace(rabbitMqSettings.Host))
            throw new InvalidOperationException("MessageBroker:Host is missing.");

        if (string.IsNullOrWhiteSpace(rabbitMqSettings.Username))
            throw new InvalidOperationException("MessageBroker:Username is missing.");

        if (string.IsNullOrWhiteSpace(rabbitMqSettings.Password))
            throw new InvalidOperationException("MessageBroker:Password is missing.");

        var virtualHost = string.IsNullOrWhiteSpace(rabbitMqSettings.VirtualHost)
            ? "/"
            : rabbitMqSettings.VirtualHost;

        var rabbitMqUri = new Uri($"rabbitmq://{rabbitMqSettings.Host}:{rabbitMqSettings.Port}/{virtualHost.TrimStart('/')}");

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

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqUri, h =>
                {
                    h.Username(rabbitMqSettings.Username);
                    h.Password(rabbitMqSettings.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
