using BuildingBlocks.Messaging.MassTransit;
using MassTransit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Orchestration.Infrastructure.Data;
using Orchestration.Infrastructure.StateMachines;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrchestrationDbConnection")
    ?? throw new InvalidOperationException("Connection string 'OrchestrationDbConnection' is missing.");

var rabbitMqSettings = builder.Configuration.GetSection("MessageBroker").Get<MessageBrokerSettings>()
    ?? throw new InvalidOperationException("MessageBroker settings are missing.");

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


//----------------

builder.Services.AddDbContext<OrchestrationDbContext>((sp, options) =>
{
    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
    options.UseNpgsql(connectionString, p =>
    {
        p.MinBatchSize(1);
    });
});

builder.Services.AddMassTransit(x =>
{
    //x.AddConsumer<OrchestrationUserRegisteredEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqUri, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });
        //cfg.ReceiveEndpoint(EventBusConstants.Queues.OrchestrationUserRegisteredEventQueueName, e =>
        //{
        //    e.ConfigureConsumer<OrchestrationUserRegisteredEventConsumer>(context);
        //});
        cfg.ReceiveEndpoint(EventBusConstants.Queues.CreateOrderMessageQueueName, e => { e.ConfigureSaga<OrderState>(context); });
    });
    x.AddEntityFrameworkOutbox<OrchestrationDbContext>(o =>
    {
        o.UsePostgres();

        o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
    });

    x.SetKebabCaseEndpointNameFormatter();
        

    x.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>()
    .EntityFrameworkRepository(r =>
    {
        r.ExistingDbContext<OrchestrationDbContext>();
        r.UsePostgres();
        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
    });

    x.AddConfigureEndpointsCallback((context, name, cfg) =>
    {
        cfg.UseEntityFrameworkOutbox<OrchestrationDbContext>(context);
    });
});

builder.Services.AddScoped<IOrchestrationDbContext, OrchestrationDbContext>();


//--------------
//Asynchronous Communication Service - Publisher  - MassTransit
//builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();
