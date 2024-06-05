using Fms.Entities;

namespace Fms.Repositories;

public interface ITransactionRepository
{
    Task<TransactionEntity> Create(TransactionEntity entity);
    Task<TransactionEntity?> Read(int id);
    Task<bool> Update(TransactionEntity entity);
    Task<bool> Delete(int id);
    
    // TODO
}