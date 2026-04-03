using Ordering.Infrastructure;
using Ordering.OutboxProcessor;
using BuildingBlocks.Messaging.MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<OrderingOutboxProcessorJob>();

//registers DB + MediatR services
builder.Services.AddInfrastuctureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(OrderingOutboxProcessorJob).Assembly));

//Asynchronous Communication Service - Publisher  - MassTransit

builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();
