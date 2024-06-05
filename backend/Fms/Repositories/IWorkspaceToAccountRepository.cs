using Fms.Entities;
using Fms.Entities.Common;

namespace Fms.Repositories;

public interface IWorkspaceToAccountRepository
{
    Task<WorkspaceToAccountEntity> Create(WorkspaceToAccountEntity entity);
    Task<WorkspaceToAccountEntity?> Read((int workspaceId, int accountId) id);
    Task<bool> Update(WorkspaceToAccountEntity entity);
    Task<bool> Delete((int workspaceId, int accountId) id);

    Task<AccountEntity?> GetOwner(int workspaceId);
    Task<WorkspaceToAccountEntity?> GetPrivateWorkspace(int accountId);
    
    Task<(int total, IEnumerable<WorkspaceToAccountEntity> items)> ListWorkspaceAccounts(int workspaceId, Pagination pagination);
    Task<(int total, IEnumerable<WorkspaceToAccountEntity> items)> ListAccountWorkspaces(int accountId, Pagination pagination);

    Task DeleteAccountFromAllOwnedBy(int accountId, int ownerAccountId);
}