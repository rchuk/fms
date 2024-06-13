using Fms.Application.Attributes;
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

public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly OrganizationRoleRepository _organizationRoleRepository;
    private readonly IOrganizationToUserRepository _organizationToUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IWorkspaceToAccountRepository _workspaceToAccountRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public OrganizationService(
        IOrganizationRepository organizationRepository, OrganizationRoleRepository organizationRoleRepository,
        IOrganizationToUserRepository organizationToUserRepository, IUserRepository userRepository,
        IAccountRepository accountRepository,
        IWorkspaceToAccountRepository workspaceToAccountRepository,
        ISubscriptionService subscriptionService,
        IAuthService authService,
        IStringLocalizer<ErrorMessages> localizer
    )
    {
        _organizationRepository = organizationRepository;
        _organizationRoleRepository = organizationRoleRepository;
        _organizationToUserRepository = organizationToUserRepository;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _workspaceToAccountRepository = workspaceToAccountRepository;
        _subscriptionService = subscriptionService;
        _authService = authService;
        _localizer = localizer;
    }
    
    [Transactional]
    public async Task<int> CreateOrganization(OrganizationUpsertRequestDto request)
    {
        if (await _subscriptionService.GetCurrentUserSubscription() is not SubscriptionKind.BusinessUnlimited)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.subscription_cant_create_organization]);
        
        if (await _organizationRepository.FindByName(request.Name) is not null)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.organization_already_exists_by_name]);
        
        var organization = await _organizationRepository.Create(new OrganizationEntity
        {
            Name = request.Name,
            Users = []
        });
        await _accountRepository.Create(new AccountEntity
        {
            OrganizationId = organization.Id
        });
        await _organizationToUserRepository.Create(new OrganizationToUserEntity
        {
            OrganizationId = organization.Id,
            UserId = await _authService.GetCurrentUserId(),
            Role = await _organizationRoleRepository.Read(OrganizationRole.Owner)
        });
        
        return organization.Id;
    }

    [Transactional]
    public async Task<OrganizationResponseDto> GetOrganization(int id)
    {
        if (await GetCurrentUserRole(id) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);

        var organizationToUser = await _organizationToUserRepository.Read((id, await _authService.GetCurrentUserId()));
        
        return BuildOrganizationResponseDto(organizationToUser!);
    }

    [Transactional]
    public async Task UpdateOrganization(int id, OrganizationUpsertRequestDto request)
    {
        if (await GetCurrentUserRole(id) is not (OrganizationRole.Admin or OrganizationRole.Owner))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);
        
        var organization = await _organizationRepository.Read(id);
        organization!.Name = request.Name; // TODO: Add merger

        if (!await _organizationRepository.Update(organization))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task DeleteOrganization(int id)
    {
        if (await GetCurrentUserRole(id) is not OrganizationRole.Owner)
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);

        if (!await _organizationRepository.Delete(id))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task AddUser(int organizationId, int userId)
    {
        if (await GetCurrentUserRole(organizationId) is not (OrganizationRole.Admin or OrganizationRole.Owner))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);

        if (await _userRepository.Read(userId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.user_doesnt_exist]);

        if (await _organizationToUserRepository.Read((organizationId, userId)) is not null)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.user_not_in_organization]);
        
        await _organizationToUserRepository.Create(new OrganizationToUserEntity
        {
            OrganizationId = organizationId,
            UserId = userId,
            Role = await _organizationRoleRepository.Read(OrganizationRole.Member)
        });
    }

    [Transactional]
    public async Task RemoveUser(int organizationId, int userId)
    {
        if (await GetCurrentUserRole(organizationId) is not (OrganizationRole.Admin or OrganizationRole.Owner))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);

        if (!await _organizationToUserRepository.Delete((organizationId, userId)))
            throw new PublicClientException(_localizer[Localization.ErrorMessages.user_not_in_organization]);
        
        var userAccount = await _accountRepository.GetUserAccount(userId);
        var organizationAccount = await _accountRepository.GetOrganizationAccount(organizationId);
        await _workspaceToAccountRepository.DeleteAccountFromAllOwnedBy(userAccount!.Id, organizationAccount!.Id);
    }

    [Transactional]
    public async Task<OrganizationRole?> GetUserRole(int organizationId, int userId)
    {
        if (await GetCurrentUserRole(organizationId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);

        return await GetUserRoleImpl(organizationId, userId);
    }
    
    [Transactional]
    public async Task UpdateUserRole(int organizationId, int userId, OrganizationRole role)
    {
        if (await GetCurrentUserRole(organizationId) is not (OrganizationRole.Admin or OrganizationRole.Owner))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);
        if (role is OrganizationRole.Owner)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.organization_cant_set_owner]);
        var currentRole = await GetUserRole(organizationId, userId);
        if (currentRole is OrganizationRole.Owner)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.organization_cant_change_owner]);
        
        var entity = await _organizationToUserRepository.Read((organizationId, userId));
        entity!.Role = await _organizationRoleRepository.Read(role);
        if (!await _organizationToUserRepository.Update(entity))
            throw new PublicClientException();
    }
    
    [Transactional]
    public async Task<OrganizationUserListResponseDto> ListOrganizationUsers(int id, UserCriteriaDto criteria, PaginationDto pagination)
    {
        if (await GetCurrentUserRole(id) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);
        
        var (total, items) = await _organizationToUserRepository.ListOrganizationUsers(id, criteria, new Pagination(pagination));

        return new OrganizationUserListResponseDto
        {
            TotalCount = total,
            Items = items.Select(BuildOrganizationUserResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<OrganizationListResponseDto> ListCurrentUserOrganizations(PaginationDto pagination)
    {
        var (total, items) = await _organizationToUserRepository.ListUserOrganizations(await _authService.GetCurrentUserId(), new Pagination(pagination));

        return new OrganizationListResponseDto
        {
            TotalCount = total,
            Items = items.Select(BuildOrganizationResponseDto).ToList()
        };
    }
    
    [Transactional]
    public async Task<OrganizationRole?> GetCurrentUserRole(int id)
    {
        var userId = await _authService.GetCurrentUserId();

        return await GetUserRoleImpl(id, userId);
    }

    private async Task<OrganizationRole?> GetUserRoleImpl(int organizationId, int userId)
    {
        if (await _organizationRepository.Read(organizationId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);
        if (await _userRepository.Read(userId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.user_doesnt_exist]);
        
        var map = await _organizationToUserRepository.Read((organizationId, userId));

        return map?.Role.ToEnum();
    }

    public static OrganizationResponseDto BuildOrganizationResponseDto(OrganizationToUserEntity entity)
    {
        return new OrganizationResponseDto
        {
            Id = entity.OrganizationId,
            Name = entity.Organization.Name,
            Role = entity.Role.ToEnum()
        };
    }
    
    public static OrganizationShortResponseDto BuildOrganizationShortResponseDto(OrganizationEntity entity)
    {
        return new OrganizationShortResponseDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    public static OrganizationUserResponseDto BuildOrganizationUserResponseDto(OrganizationToUserEntity entity)
    {
        return new OrganizationUserResponseDto
        {
            User = UserService.BuildUserResponseDto(entity.User),
            Role = entity.Role.ToEnum()
        };
    }
}