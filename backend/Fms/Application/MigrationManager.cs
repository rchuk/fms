using Microsoft.EntityFrameworkCore;

namespace Fms.Application;

public class MigrationManager
{
    public static void Migrate(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FmsDbContext>();
        dbContext.Database.Migrate();
    }
}