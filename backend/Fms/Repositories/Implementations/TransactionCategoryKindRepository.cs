using Fms.Application;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class TransactionCategoryKindRepository : BaseEnumRepository<TransactionCategoryKindEntity, TransactionCategoryKind>
{
    public TransactionCategoryKindRepository(FmsDbContext ctx) : base(ctx) {}
}