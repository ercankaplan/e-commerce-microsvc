var builder = WebApplication.CreateBuilder(args);

//Add services DI

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogDB")!);
}).UseLightweightSessions();

var app = builder.Build();

//configures the http request pipeline.

app.MapCarter();
app.Run();
