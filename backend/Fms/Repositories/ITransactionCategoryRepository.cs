using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Criteria;

namespace Fms.Repositories;

public interface ITransactionCategoryRepository
{
    Task<TransactionCategoryEntity> Create(TransactionCategoryEntity entity);
    Task<TransactionCategoryEntity?> Read(int id);
    Task<bool> Update(TransactionCategoryEntity entity);
    Task<bool> Delete(int id);
    
    Task<(int total, List<TransactionCategoryEntity>)> List(TransactionCategoryCriteria criteria, Pagination pagination);
}