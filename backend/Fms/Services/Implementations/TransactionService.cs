using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Exceptions;
using Fms.Repositories;

namespace Fms.Services.Implementations;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkspaceService _workspaceService;
    private readonly IOrganizationService _organizationService;
    private readonly IWorkspaceToAccountRepository _workspaceToAccountRepository;
    private readonly ITransactionCategoryService _transactionCategoryService;
    private readonly IAuthService _authService;
    
    public TransactionService(
        ITransactionRepository transactionRepository,
        IUserRepository userRepository,
        IWorkspaceService workspaceService,
        IOrganizationService organizationService,
        IWorkspaceToAccountRepository workspaceToAccountRepository,
        ITransactionCategoryService transactionCategoryService,
        IAuthService authService
    )
    {
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _workspaceService = workspaceService;
        _organizationService = organizationService;
        _workspaceToAccountRepository = workspaceToAccountRepository;
        _transactionCategoryService = transactionCategoryService;
        _authService = authService;
    }
    
    [Transactional]
    public async Task<int> CreateTransaction(int workspaceId, TransactionUpsertRequestDto requestDto)
    {
        await VerifyCanCreateTransaction(workspaceId);
        
        await VerifyTransactionUpsertRequest(requestDto);

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
        await VerifyCanReadTransaction(transaction);
        
        return BuildTransactionResponseDto(transaction);
    }

    [Transactional]
    public async Task UpdateTransaction(int id, TransactionUpsertRequestDto requestDto)
    {
        var transaction = await GetTransactionImpl(id);
        await VerifyCanModifyTransaction(transaction);

        await VerifyTransactionUpsertRequest(requestDto);

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

    private async Task VerifyTransactionUpsertRequest(TransactionUpsertRequestDto requestDto)
    {
        // TODO: That it returns 404
        // Create internal method
        var category = await _transactionCategoryService.GetTransactionCategory(requestDto.CategoryId);
        if (category is null)
            throw new PublicClientException();
        // TODO: Verify that category belongs to this workspace! (Or account)
        // do it manually through repo instead?
        if (requestDto.Amount == 0)
            throw new PublicClientException();
        if (category.Kind is TransactionCategoryKind.Income && requestDto.Amount < 0)
            throw new PublicClientException();
        if (category.Kind is TransactionCategoryKind.Expense && requestDto.Amount > 0)
            throw new PublicClientException();
        if (requestDto.UserId is {} userId && await _userRepository.Read(userId) is null)
            throw new PublicClientException();
    }
    
    private async Task<TransactionEntity> GetTransactionImpl(int id)
    {
        var transaction = await _transactionRepository.Read(id);
        if (transaction is null)
            throw new PublicNotFoundException();

        return transaction;
    }

    private async Task VerifyCanCreateTransaction(int workspaceId)
    {
        var workspaceOwner = await _workspaceToAccountRepository.GetOwner(workspaceId);
        if (workspaceOwner is null)
            throw new PublicNotFoundException();
        var workspaceRole = await _workspaceService.GetCurrentUserRole(workspaceId);
        if (workspaceRole is WorkspaceRole.Owner or WorkspaceRole.Admin or WorkspaceRole.Collaborator)
            return;
        
        if (workspaceOwner.Organization is { } organizationOwner)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
            if (organizationRole is null)
                throw new PublicNotFoundException();
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException();
        } else if (workspaceOwner.User is { } userOwner)
        {
            if (workspaceRole is null)
                throw new PublicNotFoundException();
        }
    }
    
    private async Task VerifyCanReadTransaction(TransactionEntity transaction)
    {
        var workspaceOwner = await _workspaceToAccountRepository.GetOwner(transaction.WorkspaceId);
        if (workspaceOwner is null)
            throw new PublicNotFoundException();
        var workspaceRole = await _workspaceService.GetCurrentUserRole(transaction.WorkspaceId);
        if (workspaceRole is not null)
            return;
        
        if (workspaceOwner.Organization is { } organizationOwner)
        {
            var organizationRole = await _organizationService.GetCurrentUserRole(organizationOwner.Id);
            if (organizationRole is null)
                throw new PublicNotFoundException();
            if (organizationRole is not (OrganizationRole.Owner or OrganizationRole.Admin))
                throw new PublicForbiddenException();
        }
        else if (workspaceOwner.User is { } userOwner)
        {
            if (workspaceRole is null)
                throw new PublicNotFoundException();
        }
    }
    
    private async Task VerifyCanModifyTransaction(TransactionEntity transaction)
    {
        await VerifyCanCreateTransaction(transaction.WorkspaceId);
    }
}