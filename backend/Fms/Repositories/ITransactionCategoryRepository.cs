using Fms.Entities;
using Fms.Entities.Common;

namespace Fms.Repositories;

public interface ITransactionCategoryRepository
{
    Task<TransactionCategoryEntity> Create(TransactionCategoryEntity entity);
    Task<TransactionCategoryEntity?> Read(int id);
    Task<bool> Update(TransactionCategoryEntity entity);
    Task<bool> Delete(int id);

    // TODO: Add search

    Task<(int total, List<TransactionCategoryEntity> items)> ListAccountCategories(int accountId, Pagination pagination);
    Task<(int total, List<TransactionCategoryEntity> items)> ListWorkspaceCategories(int workspaceId, Pagination pagination);
}