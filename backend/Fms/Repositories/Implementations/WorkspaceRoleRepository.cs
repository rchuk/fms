using Fms.Application;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class WorkspaceRoleRepository : BaseEnumRepository<WorkspaceRoleEntity, WorkspaceRole>
{
    public WorkspaceRoleRepository(FmsDbContext ctx) : base(ctx) {}
}