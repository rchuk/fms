using Fms.Entities;

namespace Fms.Repositories;

public interface IWorkspaceRepository
{
    Task<WorkspaceEntity> Create(WorkspaceEntity entity);
    Task<WorkspaceEntity?> Read(int id);
    Task<bool> Update(WorkspaceEntity entity);
    Task<bool> Delete(int id);
}