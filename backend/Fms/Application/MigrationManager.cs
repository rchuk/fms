using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fms.Application;

public class MigrationManager
{
    public static void Migrate(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FmsDbContext>();
        dbContext.Database.Migrate();
        
        AddEnums(dbContext);
    }

    private static void AddEnums(FmsDbContext db)
    {
        AddEnumVariants<OrganizationRoleEntity, OrganizationRole>(db);

        db.SaveChanges();
    }

    private static void AddEnumVariants<T, TEnum>(FmsDbContext db)
    where T: EnumEntity<TEnum>, new()
    where TEnum: struct, Enum
    {
        var dbSet = db.Set<T>();
        var existingVariants = dbSet.Select(variant => variant.Name);
        var newVariants = EnumEntity<TEnum>.GetVariants<T>()
            .Where(variant => !existingVariants.Contains(variant.Name));
        
        dbSet.AddRange(newVariants);
    }
}