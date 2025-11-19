using Ordering.API.Extensions;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add servicess to the container

builder.Services
    .AddApplicationServices()
    .AddInfrastuctureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


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
