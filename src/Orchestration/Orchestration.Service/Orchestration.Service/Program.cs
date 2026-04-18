using Orchestration.Infrastructure.Data.Extensions;
using Orchestration.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructureServices(builder.Configuration);

var host = builder.Build();
await host.InitializeDatabaseAsync();
host.Run();
