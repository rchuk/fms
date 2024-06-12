using Fms.Application;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Criteria;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class TransactionCategoryRepository : BaseCrudRepository<TransactionCategoryEntity, int>, ITransactionCategoryRepository
{
    public TransactionCategoryRepository(FmsDbContext ctx) : base(ctx) {}

    public async Task<(int total, List<TransactionCategoryEntity>)> List(TransactionCategoryCriteria criteria, Pagination pagination)
    {
        var query = Ctx.TransactionCategories.AsQueryable();
        switch (criteria)
        {
            case { WorkspaceId: {} workspaceId, AccountId: {} accountId }:
                query = query.Where(category => category.OwnerAccountId == accountId || category.WorkspaceId == workspaceId);
                break;
            case { WorkspaceId: {} workspaceId }:
                query = query.Where(category => category.WorkspaceId == workspaceId);
                break;
            case { AccountId: {} accountId }:
                query = query.Where(category => category.OwnerAccountId == accountId);
                break;
        }

        if (criteria.Query is { } searchQuery)
            query = query.Where(category => category.Name.ToLower().Contains(searchQuery.ToLower()));

        query = query.OrderBy(category => category.Id);
        
        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
}