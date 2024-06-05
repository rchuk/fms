using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Entities;

public class TransactionCategoryKindEntity : EnumEntity<TransactionCategoryKind>
{
    public TransactionCategoryKindEntity() {}
    public TransactionCategoryKindEntity(TransactionCategoryKind enumVariant) : base(enumVariant) {}
}