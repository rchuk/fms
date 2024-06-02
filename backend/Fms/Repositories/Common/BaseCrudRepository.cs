using Fms.Application;

namespace Fms.Repositories.Common;

public abstract class BaseCrudRepository<TEntity, TId> (FmsDbContext ctx, Func<TEntity, TId> idExtractor)
where TEntity: class
{
    protected FmsDbContext Ctx => ctx;
    
    public virtual async Task<TId> Create(TEntity entity)
    {
        try
        {
            var res = await ctx.AddAsync(entity);
            
            return idExtractor.Invoke(res.Entity);
        }
        finally
        {
            await ctx.SaveChangesAsync();
        }
    }

    public virtual async Task<TEntity?> Read(TId id)
    {
        if (id is IEnumerable<object> compositeId)
            return await ctx.FindAsync<TEntity>(compositeId.ToArray());
        
        return await ctx.FindAsync<TEntity>(id);
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
        ctx.Update(entity);
        
        return await ctx.SaveChangesAsync() != 0;
    }

    public virtual async Task<bool> Delete(TId id)
    {
        if (await Read(id) is {} entity)
        {
            ctx.Remove(entity);

            return await ctx.SaveChangesAsync() != 0;
        }

        return false;
    }
}