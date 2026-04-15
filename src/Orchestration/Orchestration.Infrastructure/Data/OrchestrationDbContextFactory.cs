using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Orchestration.Infrastructure.Data;

public class OrchestrationDbContextFactory : IDesignTimeDbContextFactory<OrchestrationDbContext>
{
    public OrchestrationDbContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidateBasePaths = new[]
        {
            currentDirectory,
            Path.Combine(currentDirectory, "../Orchestration.Service/Orchestration.Service"),
            Path.Combine(currentDirectory, "../../Orchestration.Service/Orchestration.Service")
        };

        var basePath = candidateBasePaths
            .Select(Path.GetFullPath)
            .FirstOrDefault(path => File.Exists(Path.Combine(path, "appsettings.json")))
            ?? throw new InvalidOperationException("Could not locate appsettings.json for design-time OrchestrationDbContext creation.");

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
