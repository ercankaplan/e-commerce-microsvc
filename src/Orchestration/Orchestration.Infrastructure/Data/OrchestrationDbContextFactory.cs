using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Orchestration.Infrastructure.Data;

public class OrchestrationDbContextFactory : IDesignTimeDbContextFactory<OrchestrationDbContext>
{
    public OrchestrationDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Orchestration.Service/Orchestration.Service");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("OrchestrationDbConnection")
            ?? throw new InvalidOperationException("Connection string 'OrchestrationDbConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<OrchestrationDbContext>();
        optionsBuilder.UseNpgsql(
            connectionString,
            npgsql => npgsql.MigrationsAssembly(typeof(OrchestrationDbContext).Assembly.FullName));

        return new OrchestrationDbContext(optionsBuilder.Options);
    }
}
