using Fms.Application;
using Fms.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Common;

public abstract class BaseEnumRepository<TEntity, TEnum> (FmsDbContext ctx)
where TEntity: EnumEntity<TEnum>, new()
where TEnum: struct, Enum
{
    protected FmsDbContext Ctx => ctx;
    
    public virtual async Task<TEntity> Read(TEnum enumVariant)
    {
        return await ctx.Set<TEntity>()
            .FirstAsync(entity => entity.Name == enumVariant.ToString().ToUpper());
    }
}