using Fms.Application;
using Fms.Entities;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class TransactionRepository : BaseCrudRepository<TransactionEntity, int>, ITransactionRepository
{
    public TransactionRepository(FmsDbContext ctx) : base(ctx) {}
}