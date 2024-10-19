using BuildingBlocks.Behaviors;
using BuildingBlocks.Handlers;

var builder = WebApplication.CreateBuilder(args);

//Add services DI


var assembly = typeof(Program).Assembly;

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
    options.Connection(builder.Configuration.GetConnectionString("CatalogDB")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddLogging();



var app = builder.Build();

//configures the http request pipeline.

app.MapCarter();

app.UseExceptionHandler(opt=> { });



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