﻿using Fms.Application.Attributes;
using Fms.Data;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Fms.Exceptions;
using Fms.Repositories;
using Fms.Repositories.Implementations;
using Microsoft.Extensions.Localization;

namespace Fms.Services.Implementations;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly WorkspaceRoleRepository _workspaceRoleRepository;
    private readonly WorkspaceKindRepository _workspaceKindRepository;
    private readonly IWorkspaceToAccountRepository _workspaceToAccountRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IAuthService _authService;
    private readonly IOrganizationService _organizationService;
    private readonly IStringLocalizer<ApplicationMessages> _localizer;
    
    public WorkspaceService(
        IWorkspaceRepository workspaceRepository,
        WorkspaceRoleRepository workspaceRoleRepository,
        WorkspaceKindRepository workspaceKindRepository,
        IWorkspaceToAccountRepository workspaceToAccountRepository,
        IAccountRepository accountRepository,
        IAuthService authService,
        IOrganizationService organizationService,
        IStringLocalizer<ApplicationMessages> localizer
    )
    {
        _workspaceRepository = workspaceRepository;
        _workspaceRoleRepository = workspaceRoleRepository;
        _workspaceKindRepository = workspaceKindRepository;
        _workspaceToAccountRepository = workspaceToAccountRepository;
        _accountRepository = accountRepository;
        _authService = authService;
        _organizationService = organizationService;
        _localizer = localizer;
    }
    
    [Transactional]
    public async Task<int> CreatePrivateUserWorkspace(int userId)
    {
        var account = await _accountRepository.GetUserAccount(userId);
        if (account is null)
            throw new PublicServerException();
        if (await _workspaceToAccountRepository.GetPrivateWorkspace(account!.Id) is not null)
            throw new PublicServerException();
    
        var workspace = await _workspaceRepository.Create(new WorkspaceEntity
        {
            Name = _localizer[Localization.Application.private_workspace_name],
            Kind = await _workspaceKindRepository.Read(WorkspaceKind.Private)
        });
        
        await _workspaceToAccountRepository.Create(new WorkspaceToAccountEntity
        {
            WorkspaceId = workspace.Id,
            AccountId = account.Id,
            Role = await _workspaceRoleRepository.Read(WorkspaceRole.Owner)
        });

        return workspace.Id;
    }

    [Transactional]
    public async Task<int> CreateSharedUserWorkspace(WorkspaceUpsertRequestDto requestDto)
    {
        // TODO: Check user's subscription model

        var workspace = await _workspaceRepository.Create(new WorkspaceEntity
        {
            Name = requestDto.Name,
            Kind = await _workspaceKindRepository.Read(WorkspaceKind.Shared)
        });
        var account = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        await _workspaceToAccountRepository.Create(new WorkspaceToAccountEntity
        {
            WorkspaceId = workspace.Id,
            AccountId = account!.Id,
            Role = await _workspaceRoleRepository.Read(WorkspaceRole.Owner)
        });

        return workspace.Id;
    }

    [Transactional]
    public async Task<int> CreateSharedOrganizationWorkspace(int organizationId, WorkspaceUpsertRequestDto requestDto)
    {
        // TODO: Check organization's subscription model
        
        var organizationRole = await _organizationService.GetCurrentUserRole(organizationId);
        if (organizationRole is null)
            throw new PublicNotFoundException();
        if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
            throw new PublicForbiddenException();

        var workspace = await _workspaceRepository.Create(new WorkspaceEntity
        {
            Name = requestDto.Name,
            Kind = await _workspaceKindRepository.Read(WorkspaceKind.Shared)
        });
        
        var organizationAccount = await _accountRepository.GetOrganizationAccount(organizationId);
        await _workspaceToAccountRepository.Create(new WorkspaceToAccountEntity
        {
            WorkspaceId = workspace.Id,
            AccountId = organizationAccount!.Id,
            Role = await _workspaceRoleRepository.Read(WorkspaceRole.Owner)
        });

        var userAccount = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        await _workspaceToAccountRepository.Create(new WorkspaceToAccountEntity
        {
            WorkspaceId = workspace.Id,
            AccountId = userAccount!.Id,
            Role = await _workspaceRoleRepository.Read(WorkspaceRole.Admin)
        });
        
        return workspace.Id;
    }

    [Transactional]
    public async Task<WorkspaceResponseDto> GetWorkspace(int id)
    {
        if (await GetCurrentUserRole(id) is null)
            throw new PublicNotFoundException();
        
        var account = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        var workspaceToUser = await _workspaceToAccountRepository.Read((id, account!.Id));

        return BuildWorkspaceResponseDto(workspaceToUser!);
    }

    [Transactional]
    public async Task UpdateWorkspace(int id, WorkspaceUpsertRequestDto requestDto)
    {
        await VerifyUserCanModifyWorkspace(id);

        var workspace = await _workspaceRepository.Read(id);
        workspace!.Name = requestDto.Name; // TODO: Add merger

        if (!await _workspaceRepository.Update(workspace))
            throw new PublicClientException();
    }
    
    [Transactional]
    public async Task DeleteWorkspace(int id)
    {
        await VerifyUserCanDeleteWorkspace(id);

        if (!await _workspaceRepository.Delete(id))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task AddUser(int workspaceId, int userId)
    {
        await VerifyUserCanModifyWorkspace(workspaceId);

        var workspace = await _workspaceRepository.Read(workspaceId);
        if (workspace!.Kind.ToEnum() is WorkspaceKind.Private)
            throw new PublicClientException();

        var account = await _accountRepository.GetUserAccount(userId);
        if (account is null)
            throw new PublicNotFoundException();

        var owner = await _workspaceToAccountRepository.GetOwner(workspaceId);
        if (owner!.OrganizationId is { } organizationOwnerId)
        {
            if (await _organizationService.GetUserRole(organizationOwnerId, userId) is null)
                throw new PublicClientException();
        }
        
        if (await _workspaceToAccountRepository.Read((workspaceId, account.Id)) is not null)
            throw new PublicClientException();
 
        await _workspaceToAccountRepository.Create(new WorkspaceToAccountEntity
        {
            WorkspaceId = workspaceId,
            AccountId = account.Id,
            Role = await _workspaceRoleRepository.Read(WorkspaceRole.Viewer)
        });
    }

    [Transactional]
    public async Task RemoveUser(int workspaceId, int userId)
    {
        if (await GetCurrentUserRole(workspaceId) is not (WorkspaceRole.Owner or WorkspaceRole.Admin))
            throw new PublicForbiddenException();

        var account = await _accountRepository.GetUserAccount(userId);
        if (account is null)
            throw new PublicNotFoundException();

        if (!await _workspaceToAccountRepository.Delete((workspaceId, account.Id)))
            throw new PublicClientException();
    }
    
    [Transactional]
    public async Task<WorkspaceRole?> GetUserRole(int workspaceId, int userId)
    {
        if (await GetCurrentUserRole(workspaceId) is null)
            throw new PublicNotFoundException();

        return await GetUserRoleImpl(workspaceId, userId);
    }
    
    [Transactional]
    public async Task UpdateUserRole(int workspaceId, int userId, WorkspaceRole role)
    {
        if (await GetCurrentUserRole(workspaceId) is not (WorkspaceRole.Owner or WorkspaceRole.Admin))
            throw new PublicForbiddenException();
        if (role is WorkspaceRole.Owner)
            throw new PublicClientException();
        var currentRole = await GetUserRole(workspaceId, userId);
        if (currentRole is WorkspaceRole.Owner)
            throw new PublicClientException();

        var account = await _accountRepository.GetUserAccount(userId);
        if (account is null)
            throw new PublicNotFoundException();

        var entity = await _workspaceToAccountRepository.Read((workspaceId, account.Id));
        entity!.Role = await _workspaceRoleRepository.Read(role);
        if (!await _workspaceToAccountRepository.Update(entity))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task<WorkspaceResponseDto> GetCurrentUserPrivateWorkspace()
    {
        var account = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        var workspace = await _workspaceToAccountRepository.GetPrivateWorkspace(account!.Id);

        return BuildWorkspaceResponseDto(workspace!);
    }

    [Transactional]
    public async Task<WorkspaceUserListResponseDto> ListWorkspaceUsers(int id, Pagination pagination)
    {
        if (await GetCurrentUserRole(id) is null)
            throw new PublicNotFoundException();

        var (total, items) = await _workspaceToAccountRepository.ListWorkspaceAccounts(id, pagination);

        return new WorkspaceUserListResponseDto
        {
            TotalCount = total,
            Items = items
                .Where(map => map.Account.UserId is not null)
                .Select(BuildWorkspaceUserResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<WorkspaceListResponseDto> ListCurrentUserWorkspaces(Pagination pagination)
    {
        var account = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        if (account is null)
            throw new PublicNotFoundException();
        
        var (total, items) = await _workspaceToAccountRepository.ListAccountWorkspaces(account.Id, pagination);
        
        return new WorkspaceListResponseDto
        {
            TotalCount = total,
            Items = items.Select(BuildWorkspaceResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<WorkspaceListResponseDto> ListOrganizationWorkspaces(int organizationId, Pagination pagination)
    {
        var account = await _accountRepository.GetOrganizationAccount(organizationId);
        if (account is null)
            throw new PublicNotFoundException();
        
        var (total, items) = await _workspaceToAccountRepository.ListAccountWorkspaces(account.Id, pagination);

        return new WorkspaceListResponseDto
        {
            TotalCount = total,
            Items = items.Select(BuildWorkspaceResponseDto).ToList()
        };
    }
    
    public async Task<WorkspaceRole?> GetCurrentUserRole(int id)
    {
        var userId = await _authService.GetCurrentUserId();

        return await GetUserRoleImpl(id, userId);
    }

    private async Task<WorkspaceRole?> GetUserRoleImpl(int workspaceId, int userId)
    {
        var account = await _accountRepository.GetUserAccount(userId);
        if (account is null)
            throw new PublicNotFoundException();
        if (await _workspaceRepository.Read(workspaceId) is null)
            throw new PublicNotFoundException();
        
        var map = await _workspaceToAccountRepository.Read((workspaceId, account.Id));

        return map?.Role.ToEnum();
    }

    public static WorkspaceResponseDto BuildWorkspaceResponseDto(WorkspaceToAccountEntity entity)
    {
        return new WorkspaceResponseDto
        {
            Id = entity.Workspace.Id,
            Name = entity.Workspace.Name,
            Kind = entity.Workspace.Kind.ToEnum(),
            Role = entity.Role.ToEnum()
        };
    }
    
    public static WorkspaceShortResponseDto BuildWorkspaceShortResponseDto(WorkspaceEntity entity)
    {
        return new WorkspaceShortResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Kind = entity.Kind.ToEnum()
        };
    }
    
    public static WorkspaceUserResponseDto BuildWorkspaceUserResponseDto(WorkspaceToAccountEntity entity)
    {
        return new WorkspaceUserResponseDto
        {
            User = UserService.BuildUserResponseDto(entity.Account.User!),
            Role = entity.Role.ToEnum()
        };
    }

    private async Task VerifyUserCanModifyWorkspace(int id)
    {
        var owner = await _workspaceToAccountRepository.GetOwner(id);
        if (owner is null)
            throw new PublicNotFoundException();

        if (owner.User is { } userOwner)
        {
            var workspaceRole = await GetCurrentUserRole(id);
            if (workspaceRole is null)
                throw new PublicNotFoundException();
            if (workspaceRole is not (WorkspaceRole.Owner or WorkspaceRole.Admin))
                throw new PublicForbiddenException();
        }
        else if (owner.Organization is { } organizationOwner)
        {
            var workspaceRole = await GetCurrentUserRole(id);
            if (workspaceRole is not WorkspaceRole.Admin)
            {
                var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
                if (organizationRole is null)
                    throw new PublicNotFoundException();
                if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                    throw new PublicForbiddenException();
            }
        }
    }
    
    private async Task VerifyUserCanDeleteWorkspace(int id)
    {
        var owner = await _workspaceToAccountRepository.GetOwner(id);
        if (owner is null)
            throw new PublicNotFoundException();

        if (owner.User is { } userOwner)
        {
            var workspaceRole = await GetCurrentUserRole(id);
            if (workspaceRole is null)
                throw new PublicNotFoundException();
            if (workspaceRole is not WorkspaceRole.Owner)
                throw new PublicForbiddenException();
        }
        else if (owner.Organization is { } organizationOwner)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
            if (organizationRole is null)
                throw new PublicNotFoundException();
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException();
        }
    }
}