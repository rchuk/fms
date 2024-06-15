using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Criteria;
using Fms.Entities.Grouped;

namespace Fms.Repositories;

public interface IWorkspaceToAccountRepository
{
    Task<WorkspaceToAccountEntity> Create(WorkspaceToAccountEntity entity);
    Task<WorkspaceToAccountEntity?> Read((int workspaceId, int accountId) id);
    Task<bool> Update(WorkspaceToAccountEntity entity);
    Task<bool> Delete((int workspaceId, int accountId) id);

    Task<AccountEntity?> GetOwner(int workspaceId);
    Task<WorkspaceWithOwner?> GetWithOwner(int workspaceId, int accountId);
    Task<WorkspaceWithOwner?> GetPrivateWorkspace(int accountId);
    
    Task<(int total, IEnumerable<WorkspaceToAccountEntity> items)> ListWorkspaceAccounts(int workspaceId, AccountCriteria accountCriteria, Pagination pagination);
    Task<(int total, IEnumerable<WorkspaceWithOwner> items)> ListAccountWorkspaces(int accountId, Pagination pagination);

    Task DeleteAccountFromAllOwnedBy(int accountId, int ownerAccountId);
}