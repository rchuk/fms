using Fms.Application;
using Fms.Entities;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class WorkspaceRepository : BaseCrudRepository<WorkspaceEntity, int>, IWorkspaceRepository
{
    public WorkspaceRepository(FmsDbContext ctx) : base(ctx) {}
}