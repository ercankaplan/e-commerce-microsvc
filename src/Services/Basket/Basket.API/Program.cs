using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Handlers;
using Marten;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

//Add services to container########################

var assembly = typeof(Program).Assembly;
var connStrBasketDB = builder.Configuration.GetConnectionString("BasketDB");
var redisConStr = builder.Configuration.GetConnectionString("Redis");


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

/* manually DI CachedBasketRepository
builder.Services.AddScoped(provider=> {

    var basketRepository = provider.GetRequiredService<BasketRepository>();
    var distributedCache = provider.GetRequiredService<IDistributedCache>();

    return new CachedBasketRepository(basketRepository, distributedCache);

});
*/
builder.Services.Decorate<IBasketRepository,CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "BasketAPICache";
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline ##########################

app.UseExceptionHandler(opt => { });

app.MapCarter();

app.Run();
