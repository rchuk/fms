using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Entities;

public class WorkspaceKindEntity : EnumEntity<WorkspaceKind>
{
    public WorkspaceKindEntity() {}
    public WorkspaceKindEntity(WorkspaceKind enumVariant) : base(enumVariant) {}
}