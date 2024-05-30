using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fms.Application;

public class FmsDbContextDesignFactory : IDesignTimeDbContextFactory<FmsDbContext>
{
    public FmsDbContext CreateDbContext(string[] args)
    {
        /*
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        */
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var name = Environment.GetEnvironmentVariable("POSTGRES_NAME");
        var user = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var connection = $"Host={host};Database={name};Username={user};Password={password};Port={port}";
        
        var optionsBuilder = new DbContextOptionsBuilder<FmsDbContext>()
            .UseNpgsql(connection);

        return new FmsDbContext(optionsBuilder.Options);
    }
}

