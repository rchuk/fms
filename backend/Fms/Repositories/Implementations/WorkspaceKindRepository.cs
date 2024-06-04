using Fms.Application;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class WorkspaceKindRepository : BaseEnumRepository<WorkspaceKindEntity, WorkspaceKind>
{
    public WorkspaceKindRepository(FmsDbContext ctx) : base(ctx) {}
}