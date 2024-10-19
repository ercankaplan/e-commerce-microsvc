using BuildingBlocks.Behaviors;
using BuildingBlocks.Handlers;
using Catalog.API.Data;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

//Add services DI


var assembly = typeof(Program).Assembly;
var connStrCatalogDB = builder.Configuration.GetConnectionString("CatalogDB");

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(connStrCatalogDB!);
}).UseLightweightSessions();


//Seeding data
if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddLogging();

builder.Services.AddHealthChecks() //healtcheck api
    .AddNpgSql(connStrCatalogDB!); //healthcheck background dependency to db

var app = builder.Build();

//configures the http request pipeline.###################################

app.MapCarter();

app.UseExceptionHandler(opt=> { });

app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions() {
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});



app.UseHsts();

app.Run();



/* 
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {


        var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

        if (exception == null)
        {
            return;
        }

        var exceptionDetail = new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace,
            Title = exception.Message
        };

        var logger = context.RequestServices.GetService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";


        await context.Response.WriteAsJsonAsync(exceptionDetail);
    });
});

*/