using Ordering.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

//Add servicess to the container

builder.Services
    .AddApplicationServices()
    .AddInfrastuctureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();

//Configure the Http Request pipeline.

app.UseApiServices();

app.MapGet("/", () => "Hello World!");

app.Run();
