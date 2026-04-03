using Basket.OutboxProcessor;
using Marten;
using MassTransit;
using Weasel.Core;
using BuildingBlocks.Messaging.MassTransit;
using MediatR;  

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<BasketOutboxProcessorJob>();

var connStrBasketDB = builder.Configuration.GetConnectionString("BasketDB");
//registers DB + MediatR services
builder.Services.AddMarten(options =>
{
    options.Connection(connStrBasketDB!);
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
}).UseLightweightSessions();

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(BasketOutboxProcessorJob).Assembly));

//Asynchronous Communication Service - Publisher  - MassTransit
builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();