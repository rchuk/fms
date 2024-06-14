using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Grouped;

namespace Fms.Repositories;

public interface ITransactionRepository
{
    Task<TransactionEntity> Create(TransactionEntity entity);
    Task<TransactionEntity?> Read(int id);
    Task<bool> Update(TransactionEntity entity);
    Task<bool> Delete(int id);

    Task<(int total, IEnumerable<TransactionEntity> items)> ListWorkspaceTransactions(int workspaceId,
        TransactionCriteriaDto criteria, Pagination pagination);

    Task<TransactionGroupedByCategoryList> ListWorkspaceTransactionsGroupedByCategory(int workspaceId,
        TransactionCriteriaDto criteria);
    Task<TransactionGroupedByUserList> ListWorkspaceTransactionsGroupedByUser(int workspaceId,
        TransactionCriteriaDto criteria);
}