using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Services;

public interface IWorkspaceService
{
    public Task<int> CreatePrivateUserWorkspace(int userId);
    public Task<int> CreateSharedUserWorkspace(WorkspaceUpsertRequestDto requestDto);
    public Task<int> CreateSharedOrganizationWorkspace(int organizationId, WorkspaceUpsertRequestDto requestDto);
    public Task<WorkspaceResponseDto> GetWorkspace(int id);
    public Task UpdateWorkspace(int id, WorkspaceUpsertRequestDto requestDto);
    public Task DeleteWorkspace(int id);
    
    public Task AddUser(int workspaceId, int userId);
    public Task RemoveUser(int workspaceId, int userId);
    public Task<WorkspaceRole?> GetUserRole(int workspaceId, int userId);
    public Task UpdateUserRole(int workspaceId, int userId, WorkspaceRole role);

    public Task<WorkspaceResponseDto> GetCurrentUserPrivateWorkspace();
    
    public Task<WorkspaceUserListResponseDto> ListWorkspaceUsers(int workspaceId, Pagination pagination);
    public Task<WorkspaceListResponseDto> ListCurrentUserWorkspaces(Pagination pagination);
    public Task<WorkspaceListResponseDto> ListOrganizationWorkspaces(int organizationId, Pagination pagination);
}