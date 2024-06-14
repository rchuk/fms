using System.Text.RegularExpressions;
using Fms.Application.Attributes;
using Fms.Data;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Criteria;
using Fms.Entities.Enums;
using Fms.Exceptions;
using Fms.Repositories;
using Fms.Repositories.Implementations;
using Fms.Services.Implementations;
using Microsoft.Extensions.Localization;

namespace Fms.Services;

public partial class TransactionCategoryService : ITransactionCategoryService
{
    private static Regex _colorRegex = ColorRegex();
    
    private readonly ITransactionCategoryRepository _transactionCategoryRepository;
    private readonly TransactionCategoryKindRepository _transactionCategoryKindRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IWorkspaceToAccountRepository _workspaceToAccountRepository;
    private readonly IOrganizationService _organizationService;
    private readonly IWorkspaceService _workspaceService;
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<ErrorMessages> _localizer;

    public TransactionCategoryService(
        ITransactionCategoryRepository transactionCategoryRepository,
        TransactionCategoryKindRepository transactionCategoryKindRepository,
        IWorkspaceToAccountRepository workspaceToAccountRepository,
        IAccountRepository accountRepository,
        IOrganizationService organizationService,
        IWorkspaceService workspaceService,
        IAuthService authService,
        IStringLocalizer<ErrorMessages> localizer
    )
    {
        _transactionCategoryRepository = transactionCategoryRepository;
        _transactionCategoryKindRepository = transactionCategoryKindRepository;
        _accountRepository = accountRepository;
        _workspaceToAccountRepository = workspaceToAccountRepository;
        _organizationService = organizationService;
        _workspaceService = workspaceService;
        _authService = authService;
        _localizer = localizer;
    }

    [Transactional]
    public async Task<int> CreateUserTransactionCategory(TransactionCategoryUpsertRequestDto request)
    {
        if (request.UiColor is {} uiColor && !IsValidColor(uiColor))
            throw new PublicClientException(_localizer[Localization.ErrorMessages.invalid_color]);
        
        var account = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        var entity = await _transactionCategoryRepository.Create(new TransactionCategoryEntity
        {
            OwnerAccountId = account!.Id,
            Name = request.Name,
            Kind = await _transactionCategoryKindRepository.Read(request.Kind),
            UiColor = request.UiColor
        });

        return entity.Id;
    }

    [Transactional]
    public async Task<int> CreateOrganizationTransactionCategory(int organizationId, TransactionCategoryUpsertRequestDto request)
    {
        if (request.UiColor is {} uiColor && !IsValidColor(uiColor))
            throw new PublicClientException(_localizer[Localization.ErrorMessages.invalid_color]);
        
        var organizationRole = await _organizationService.GetCurrentUserRole(organizationId);
        if (organizationRole is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);
        if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);
        
        var account = await _accountRepository.GetOrganizationAccount(organizationId);
        var entity = await _transactionCategoryRepository.Create(new TransactionCategoryEntity
        {
            OwnerAccountId = account!.Id,
            Name = request.Name,
            Kind = await _transactionCategoryKindRepository.Read(request.Kind),
            UiColor = request.UiColor
        });

        return entity.Id;
    }

    [Transactional]
    public async Task<int> CreateWorkspaceTransactionCategory(int workspaceId, TransactionCategoryUpsertRequestDto request)
    {
        if (request.UiColor is {} uiColor && !IsValidColor(uiColor))
            throw new PublicClientException(_localizer[Localization.ErrorMessages.invalid_color]);
        
        var workspaceRole = await _workspaceService.GetCurrentUserRole(workspaceId);
        if (workspaceRole is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
        if (workspaceRole is not (WorkspaceRole.Owner or WorkspaceRole.Admin or WorkspaceRole.Collaborator))
            throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.workspace_forbidden]);

        var workspaceOwnerAccount = await _workspaceToAccountRepository.GetOwner(workspaceId);
        var entity = await _transactionCategoryRepository.Create(new TransactionCategoryEntity
        {
            OwnerAccountId = workspaceOwnerAccount!.Id,
            WorkspaceId = workspaceId,
            Name = request.Name,
            Kind = await _transactionCategoryKindRepository.Read(request.Kind),
            UiColor = request.UiColor
        });

        return entity.Id;
    }

    [Transactional]
    public async Task<TransactionCategoryResponseDto> GetTransactionCategory(int id)
    {
        var category = await _transactionCategoryRepository.Read(id);
        if (category is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);

        await VerifyCanRead(category);

        return BuildTransactionCategoryResponseDto(category);
    }

    [Transactional]
    public async Task UpdateTransactionCategory(int id, TransactionCategoryUpsertRequestDto request)
    {
        if (request.UiColor is {} uiColor && !IsValidColor(uiColor))
            throw new PublicClientException(_localizer[Localization.ErrorMessages.invalid_color]);
        
        var category = await _transactionCategoryRepository.Read(id);
        if (category is null)
            throw new PublicNotFoundException(); 

        await VerifyCanModify(category);

        category.Name = request.Name; // TODO: Add merger
        category.Kind = await _transactionCategoryKindRepository.Read(request.Kind);
        if (request.UiColor is not null)
            category.UiColor = request.UiColor;

        if (!await _transactionCategoryRepository.Update(category))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task DeleteTransactionCategory(int id)
    {
        var category = await _transactionCategoryRepository.Read(id);
        if (category is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]); 

        await VerifyCanModify(category);

        // TODO: Add constraint so we can't delete, while transactions using this category are present
        if (!await _transactionCategoryRepository.Delete(id))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task<TransactionCategoryListResponseDto> ListUserTransactionCategories(TransactionCategoryCriteriaDto criteriaDto, PaginationDto pagination)
    {
        var userAccount = await _accountRepository.GetUserAccount(await _authService.GetCurrentUserId());
        var criteria = new TransactionCategoryCriteria
        {
            AccountId = userAccount!.Id,
            Query = criteriaDto.Query
        };
        var (total, items) = await _transactionCategoryRepository.List(criteria, new Pagination(pagination));

        return new TransactionCategoryListResponseDto
        { 
            TotalCount = total,
            Items = items.Select(BuildTransactionCategoryResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<TransactionCategoryListResponseDto> ListOrganizationTransactionCategories(TransactionCategoryCriteriaDto criteriaDto, int organizationId, PaginationDto pagination)
    {
        if (await _organizationService.GetCurrentUserRole(organizationId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.organization_doesnt_exist]);
        
        var organizationAccount = await _accountRepository.GetOrganizationAccount(organizationId);
        if (organizationAccount is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.account_doesnt_exist]);
        var criteria = new TransactionCategoryCriteria
        {
            AccountId = organizationAccount.Id,
            Query = criteriaDto.Query
        };
        var (total, items) = await _transactionCategoryRepository.List(criteria, new Pagination(pagination));
        
        return new TransactionCategoryListResponseDto
        { 
            TotalCount = total,
            Items = items.Select(BuildTransactionCategoryResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<TransactionCategoryListResponseDto> ListWorkspaceTransactionCategories(TransactionCategoryCriteriaDto criteriaDto, int workspaceId, PaginationDto pagination)
    {
        if (await _workspaceService.GetCurrentUserRole(workspaceId) is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);

        var criteria = new TransactionCategoryCriteria
        {
            AccountId = criteriaDto.IncludeOwner.GetValueOrDefault(false) ? (await _workspaceToAccountRepository.GetOwner(workspaceId))!.Id : null,
            WorkspaceId = workspaceId,
            Query = criteriaDto.Query
        };
        var (total, items) = await _transactionCategoryRepository.List(criteria, new Pagination(pagination));
        
        return new TransactionCategoryListResponseDto
        { 
            TotalCount = total,
            Items = items.Select(BuildTransactionCategoryResponseDto).ToList()
        };
    }

    private async Task VerifyCanRead(TransactionCategoryEntity category)
    {
        if (category.Workspace is { } workspace)
        {
            var workspaceRole = await _workspaceService.GetCurrentUserRole(workspace.Id);
            if (workspaceRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
        else if (category.OwnerAccount.OrganizationId is { } organizationOwnerId)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwnerId);
            if (organizationRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
        else if (category.OwnerAccount.UserId is { } userOwnerId)
        {
            if (userOwnerId != await _authService.GetCurrentUserId())
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
    }
    
    private async Task VerifyCanModify(TransactionCategoryEntity category)
    {
        if (category.Workspace is { } workspace)
        {
            var workspaceRole = await _workspaceService.GetCurrentUserRole(workspace.Id);
            if (workspaceRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
            if (workspaceRole is not (WorkspaceRole.Owner or WorkspaceRole.Admin or WorkspaceRole.Collaborator))
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.workspace_forbidden]);
        }
        else if (category.OwnerAccount.OrganizationId is { } organizationOwnerId)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwnerId);
            if (organizationRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.organization_forbidden]);
        }
        else if (category.OwnerAccount.UserId is { } userOwnerId)
        {
            if (userOwnerId != await _authService.GetCurrentUserId())
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
    }

    public static TransactionCategoryResponseDto BuildTransactionCategoryResponseDto(TransactionCategoryEntity entity)
    {
        return new TransactionCategoryResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            OwnerAccount = AccountService.BuildAccountResponseDto(entity.OwnerAccount),
            Kind = entity.Kind.ToEnum(),
            UiColor = entity.UiColor,
            Workspace = entity.Workspace != null ? WorkspaceService.BuildWorkspaceShortResponseDto(entity.Workspace) : null
        };
    }

    private static bool IsValidColor(string color)
    {
        return color.Length != 0 && _colorRegex.IsMatch(color);
    }

    [GeneratedRegex(@"^[0-9A-Fa-f]{6}$")]
    private static partial Regex ColorRegex();
}
