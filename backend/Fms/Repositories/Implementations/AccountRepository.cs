using Fms.Application;
using Fms.Entities;

namespace Fms.Repositories.Implementations;

public class AccountRepository (FmsDbContext ctx) : IAccountRepository
{
    protected FmsDbContext Ctx { get; } = ctx;
    
    public async Task<AccountEntity> Create(AccountEntity entity)
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

    public async Task<AccountEntity?> Read(int id)
    {
        return await Ctx.FindAsync<AccountEntity>(id);
    }
}