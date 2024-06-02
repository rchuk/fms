using System.Runtime.CompilerServices;
using Fms.Application;

namespace Fms.Repositories.Common;

public abstract class BaseCrudRepository<TEntity, TId>
where TEntity: class
{
    protected FmsDbContext Ctx { get; }

    private readonly Func<TId, Task<TEntity?>> _readFn;

    protected BaseCrudRepository(FmsDbContext ctx)
    {
        Ctx = ctx;
        _readFn = typeof(TId).GetInterfaces().Contains(typeof(ITuple))
            ? async id => await Ctx.FindAsync<TEntity>(TupleExtract((ITuple?)id).ToArray())
            : async id => await Ctx.FindAsync<TEntity>(id);
    }
    
    public virtual async Task<TEntity> Create(TEntity entity)
    {
        try
        {
            var res = await Ctx.AddAsync(entity);

            return res.Entity;
        }
        finally
        {
            await Ctx.SaveChangesAsync();
        }
    }

    public virtual async Task<TEntity?> Read(TId id)
    {
        return await _readFn(id);
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
        Ctx.Update(entity);
        
        return await Ctx.SaveChangesAsync() != 0;
    }

    public virtual async Task<bool> Delete(TId id)
    {
        if (await Read(id) is {} entity)
        {
            Ctx.Remove(entity);

            return await Ctx.SaveChangesAsync() != 0;
        }

        return false;
    }

    private static IEnumerable<object?> TupleExtract(ITuple? tuple)
    {
        if (tuple == null)
            yield break;
        
        for (var i = 0; i < tuple.Length; ++i)
            yield return tuple[i];
    }
}