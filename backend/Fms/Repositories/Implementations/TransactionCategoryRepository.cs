using Fms.Application;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class TransactionCategoryRepository : BaseCrudRepository<TransactionCategoryEntity, int>, ITransactionCategoryRepository
{
    public TransactionCategoryRepository(FmsDbContext ctx) : base(ctx) {}

    public async Task<(int total, List<TransactionCategoryEntity> items)> ListAccountCategories(int accountId, Pagination pagination)
    {
        var query = Ctx.TransactionCategories
            .Where(category => category.OwnerAccountId == accountId);

        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }

    public async Task<(int total, List<TransactionCategoryEntity> items)> ListWorkspaceCategories(int workspaceId, Pagination pagination)
    {
        var query = Ctx.TransactionCategories
            .Where(category => category.WorkspaceId == workspaceId);

        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
}