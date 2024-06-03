using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Entities;

public class WorkspaceRoleEntity : EnumEntity<WorkspaceRole>
{
    public WorkspaceRoleEntity() {}
    public WorkspaceRoleEntity(WorkspaceRole enumVariant) : base(enumVariant) {}
}