using Fms.Application;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class SubscriptionKindRepository : BaseEnumRepository<SubscriptionKindEntity, SubscriptionKind>
{
    public SubscriptionKindRepository(FmsDbContext ctx) : base(ctx) {}
}