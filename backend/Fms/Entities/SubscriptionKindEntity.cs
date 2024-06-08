using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Entities;

public class SubscriptionKindEntity : EnumEntity<SubscriptionKind>
{
    public SubscriptionKindEntity() {}
    public SubscriptionKindEntity(SubscriptionKind enumVariant) : base(enumVariant) {}
}