using Fms.Application.Attributes;
using Fms.Data;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Fms.Exceptions;
using Fms.Repositories;
using Microsoft.Extensions.Localization;

namespace Fms.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceService _workspaceService;
    private readonly IOrganizationService _organizationService;
    private readonly IWorkspaceToAccountRepository _workspaceToAccountRepository;
    private readonly ITransactionCategoryRepository _transactionCategoryRepository;
    private readonly IAuthService _authService;
    private readonly IStringLocalizer<ErrorMessages> _localizer;
    
    public TransactionService(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IWorkspaceService workspaceService,
        IOrganizationService organizationService,
        IWorkspaceToAccountRepository workspaceToAccountRepository,
        ITransactionCategoryRepository transactionCategoryRepository,
        IAuthService authService,
        IStringLocalizer<ErrorMessages> localizer
    )
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _workspaceService = workspaceService;
        _organizationService = organizationService;
        _workspaceToAccountRepository = workspaceToAccountRepository;
        _transactionCategoryRepository = transactionCategoryRepository;
        _authService = authService;
        _localizer = localizer;
    }
    
    [Transactional]
    public async Task<int> CreateTransaction(int workspaceId, TransactionUpsertRequestDto requestDto)
    {
        await VerifyCanCreateTransaction(workspaceId);
        
        await VerifyTransactionUpsertRequest(workspaceId, requestDto);

        var creationTimestamp = DateTime.UtcNow;
        var transaction = await _transactionRepository.Create(new TransactionEntity
        {
            WorkspaceId = workspaceId,
            CategoryId = requestDto.CategoryId,
            Amount = requestDto.Amount,
            Description = requestDto.Description,
            Timestamp = requestDto.Timestamp ?? creationTimestamp,
            CreationTimestamp = creationTimestamp,
            CreationUserId = await _authService.GetCurrentUserId(),
            UserId = requestDto.UserId
        });

        return transaction.Id;
    }

    [Transactional]
    public async Task<TransactionResponseDto> GetTransaction(int id)
    {
        var transaction = await GetTransactionImpl(id);
        await VerifyCanReadTransaction(transaction.WorkspaceId);
        
        return BuildTransactionResponseDto(transaction);
    }

    [Transactional]
    public async Task UpdateTransaction(int id, TransactionUpsertRequestDto requestDto)
    {
        var transaction = await GetTransactionImpl(id);
        await VerifyCanModifyTransaction(transaction);

        var category = await _transactionCategoryRepository.Read(requestDto.CategoryId);
        if (category is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);

        await VerifyTransactionCategorySuitable(category, transaction.WorkspaceId);

        transaction.CategoryId = requestDto.CategoryId;
        transaction.Amount = requestDto.Amount;
        transaction.Description = requestDto.Description;
        transaction.Timestamp = requestDto.Timestamp ?? transaction.Timestamp;
        transaction.UserId = requestDto.UserId;

        if (!await _transactionRepository.Update(transaction))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task DeleteTransaction(int id)
    {
        var transaction = await GetTransactionImpl(id);
        await VerifyCanModifyTransaction(transaction);

        if (!await _transactionRepository.Delete(id))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task<TransactionListResponseDto> ListWorkspaceTransactions(int workspaceId, TransactionCriteriaDto criteria, PaginationDto pagination)
    {
        await VerifyCanReadTransaction(workspaceId);
        
        var (count, items) = await _transactionRepository.ListWorkspaceTransactions(workspaceId, criteria, new Pagination(pagination));

        return new TransactionListResponseDto
        {
            TotalCount = count,
            Items = items.Select(BuildTransactionResponseDto).ToList()
        };
    }
    
    public static TransactionResponseDto BuildTransactionResponseDto(TransactionEntity transaction)
    {
        return new TransactionResponseDto
        {
            Id = transaction.Id,
            Workspace = WorkspaceService.BuildWorkspaceShortResponseDto(transaction.Workspace),
            Category = TransactionCategoryService.BuildTransactionCategoryResponseDto(transaction.Category),
            Amount = transaction.Amount,
            Description = transaction.Description,
            Timestamp = transaction.Timestamp,
            CreationTimestamp = transaction.CreationTimestamp,
            CreationUser = UserService.BuildUserResponseDto(transaction.CreationUser),
            User = transaction.User != null ? UserService.BuildUserResponseDto(transaction.User) : null
        };
    }

    private async Task VerifyTransactionUpsertRequest(int workspaceId, TransactionUpsertRequestDto requestDto)
    {
        var category = await _transactionCategoryRepository.Read(requestDto.CategoryId);
        if (category is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);

        await VerifyTransactionCategorySuitable(category, workspaceId);
        
        if (requestDto.Amount == 0)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.transaction_amount_zero]);
        if (category.Kind.ToEnum() is TransactionCategoryKind.Income && requestDto.Amount < 0)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.transaction_amount_doesnt_match_category]);
        if (category.Kind.ToEnum() is TransactionCategoryKind.Expense && requestDto.Amount > 0)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.transaction_amount_doesnt_match_category]);
        if (requestDto.UserId is {} userId && await _userRepository.Read(userId) is null)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.user_doesnt_exist]);
    }
    
    private async Task VerifyTransactionCategorySuitable(TransactionCategoryEntity category, int workspaceId)
    {
        if (category.WorkspaceId is { } categoryWorkspaceId && categoryWorkspaceId != workspaceId)
            throw new PublicClientException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);

        if (category.OwnerAccount.Organization is { } organizationOwner)
        {
            if (await _organizationService.GetCurrentUserRole(organizationOwner.Id) is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
        else if (category.OwnerAccount.User is { } userOwner)
        {
            if (userOwner.Id != await _authService.GetCurrentUserId())
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_category_doesnt_exist]);
        }
    }
    
    private async Task<TransactionEntity> GetTransactionImpl(int id)
    {
        var transaction = await _transactionRepository.Read(id);
        if (transaction is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.transaction_doesnt_exist]);

        return transaction;
    }

    private async Task VerifyCanCreateTransaction(int workspaceId)
    {
        var workspaceOwner = await _workspaceToAccountRepository.GetOwner(workspaceId);
        if (workspaceOwner is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.account_doesnt_exist]);
        var workspaceRole = await _workspaceService.GetCurrentUserRole(workspaceId);
        if (workspaceRole is WorkspaceRole.Owner or WorkspaceRole.Admin or WorkspaceRole.Collaborator)
            return;
        
        if (workspaceOwner.Organization is { } organizationOwner)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
            if (organizationRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.workspace_forbidden]);
        } else if (workspaceOwner.User is { } userOwner)
        {
            if (workspaceRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
        }
    }
    
    private async Task VerifyCanReadTransaction(int workspaceId)
    {
        var workspaceOwner = await _workspaceToAccountRepository.GetOwner(workspaceId);
        if (workspaceOwner is null)
            throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
        var workspaceRole = await _workspaceService.GetCurrentUserRole(workspaceId);
        if (workspaceRole is not null)
            return;
        
        if (workspaceOwner.Organization is { } organizationOwner)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
            if (organizationRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException(_localizer[Localization.ErrorMessages.workspace_forbidden]);
        }
        else if (workspaceOwner.User is { } userOwner)
        {
            if (workspaceRole is null)
                throw new PublicNotFoundException(_localizer[Localization.ErrorMessages.workspace_doesnt_exist]);
        }
    }
    
    private async Task VerifyCanModifyTransaction(TransactionEntity transaction)
    {
        await VerifyCanCreateTransaction(transaction.WorkspaceId);
    }
}