using Basket.API.Data;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Handlers;
using BuildingBlocks.Messaging.MassTransit;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Marten;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

//Add services to container########################

var assembly = typeof(Program).Assembly;
var connStrBasketDB = builder.Configuration.GetConnectionString("BasketDB");
var connStrRedis = builder.Configuration.GetConnectionString("Redis");


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
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketOutboxMessageRepository, BasketOutboxMessageRepository>();
builder.Services.AddScoped<IBasketUnitOfWork, BasketUnitOfWork>();

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
    options.Configuration = connStrRedis;
    //options.InstanceName = "BasketAPICache";
});

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{

    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);

}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

//Asynchronous Communication Service - Publisher  - MassTransit

//builder.Services.AddMessageBroker(builder.Configuration);

//Cross cutting concerns

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks() //healtcheck api
      .AddNpgSql(connStrBasketDB!)//healtcheck postgre
      .AddRedis(connStrRedis!); //healtcheck redis


var app = builder.Build();

//Configure the HTTP request pipeline ##########################

app.UseExceptionHandler(opt => { });

app.MapCarter();

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
