using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Handlers;
using Marten;

var builder = WebApplication.CreateBuilder(args);

//Add services to container########################

var assembly = typeof(Program).Assembly;
var connStrBasketDB = builder.Configuration.GetConnectionString("BasketDB");


builder.Services.AddMediatR(config=> {

    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();


builder.Services.AddMarten(options =>
{
    options.Connection(connStrBasketDB!);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline ##########################

app.UseExceptionHandler(opt => { });

app.MapCarter();

app.Run();
