using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities;
using Fms.Exceptions;
using Fms.Repositories;

namespace Fms.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IOrganizationToUserRepository _organizationToUserRepository;
    private readonly IAuthService _authService;
    
    public AccountService(
        IAccountRepository accountRepository, IOrganizationToUserRepository organizationToUserRepository,
        IAuthService authService
        )
    {
        _accountRepository = accountRepository;
        _organizationToUserRepository = organizationToUserRepository;
        _authService = authService;
    }
    
    [Transactional]
    public async Task<AccountResponseDto> GetAccount(int id)
    {
        var account = await _accountRepository.Read(id);
        if (account is null)
            throw new PublicNotFoundException();

        var currentUserId = await _authService.GetCurrentUserId();
        
        if (account.UserId is {} userId)
        {
            if (!await _organizationToUserRepository.AreRelatedUsers(currentUserId, userId))
                throw new PublicNotFoundException();
        } else if (account.OrganizationId is { } organizationId)
        {
            if (await _organizationToUserRepository.Read((organizationId, currentUserId)) is null)
                throw new PublicNotFoundException();
        }

        return BuildAccountResponseDto(account);
    }

    public static AccountResponseDto BuildAccountResponseDto(AccountEntity account)
    {
        return new AccountResponseDto
        {
            Organization = account.Organization != null ? OrganizationService.BuildOrganizationShortResponseDto(account.Organization) : null,
            User = account.User != null ? UserService.BuildUserResponseDto(account.User) : null
        };
    }
}