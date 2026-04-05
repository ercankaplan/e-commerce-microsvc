using Ordering.Application;
using Ordering.Application.Orders.Commands;
using Ordering.InboxProcessor;
using Ordering.Infrastructure;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<OrderingInboxProcessorJob>();

//registers DB + MediatR services
builder.Services.AddInfrastuctureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

//Asynchronous Communication Service - Publisher  - MassTransit

//builder.Services.AddMessageBroker(builder.Configuration);

var host = builder.Build();
host.Run();
