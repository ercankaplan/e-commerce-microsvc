using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

;

var builder = WebApplication.CreateBuilder(args);

//Add servicess to the container

builder.Services
    .AddApplicationServices(builder.Configuration) // Application Layer services (MediatR, MassTransit)
    .AddInfrastuctureServices(builder.Configuration) // Infrastucture Layer services (DbContext, Repositories)
    .AddApiServices(builder.Configuration); // Presentation Layer services (Carter, Health Checks, Exception Handling)

var app = builder.Build();

//Configure the Http Request pipeline.

app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    await app.SeedDataAsync();
}

app.MapGet("/", () => "Hello World!");

app.Run();
