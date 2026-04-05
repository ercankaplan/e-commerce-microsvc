using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data;
using Ordering.OutboxProcessor;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<OrderingOutboxProcessorJob>();

//registers DB + MediatR services
builder.Services.AddInfrastuctureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(OrderingOutboxProcessorJob).Assembly));

//Asynchronous Communication Service - Publisher  - MassTransit

builder.Services.AddMessageBroker(builder.Configuration);

builder.Services.AddHealthChecks()
    .AddCheck<OutboxHealthCheck>("outbox");


var host = builder.Build();
host.Run();
