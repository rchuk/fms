﻿using Fms.Application;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class WorkspaceToAccountRepository : BaseCrudRepository<WorkspaceToAccountEntity, (int workspaceId, int accountId)>, IWorkspaceToAccountRepository
{
    private readonly WorkspaceRoleRepository _workspaceRoleRepository;
    private readonly WorkspaceKindRepository _workspaceKindRepository;

    public WorkspaceToAccountRepository(
        FmsDbContext ctx,
        WorkspaceRoleRepository workspaceRoleRepository,
        WorkspaceKindRepository workspaceKindRepository
    ) : base(ctx)
    {
        _workspaceRoleRepository = workspaceRoleRepository;
        _workspaceKindRepository = workspaceKindRepository;
    }

    public async Task<AccountEntity?> GetOwner(int workspaceId)
    {
        var role = await _workspaceRoleRepository.Read(WorkspaceRole.Owner);
        var result = await Ctx.WorkspaceToAccount
            .Where(map => map.WorkspaceId == workspaceId)
            .FirstOrDefaultAsync(map => map.Role.Id == role.Id);

        return result?.Account;
    }

    public async Task<WorkspaceToAccountEntity?> GetPrivateWorkspace(int accountId)
    {
        var kind = await _workspaceKindRepository.Read(WorkspaceKind.Private);
        return await Ctx.WorkspaceToAccount
            .Where(map => map.AccountId == accountId)
            .Where(map => map.Workspace.Kind == kind)
            .FirstOrDefaultAsync();
    }
    
    public async Task<(int total, IEnumerable<WorkspaceToAccountEntity> items)> ListWorkspaceAccounts(int workspaceId, Pagination pagination)
    {
        var query = Ctx.WorkspaceToAccount
            .Where(map => map.WorkspaceId == workspaceId)
            .OrderBy(map => map.AccountId);

        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }

    public async Task<(int total, IEnumerable<WorkspaceToAccountEntity> items)> ListAccountWorkspaces(int accountId, Pagination pagination)
    {
        var query = Ctx.WorkspaceToAccount
            .Where(map => map.AccountId == accountId)
            .OrderBy(map => map.WorkspaceId)
            .Include(map => map.Workspace);

        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
}