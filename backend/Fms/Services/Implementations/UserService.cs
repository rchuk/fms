using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities;
using Fms.Repositories;

namespace Fms.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly Lazy<IWorkspaceService> _workspaceService;

    public UserService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        IServiceProvider services
    )
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _workspaceService = new Lazy<IWorkspaceService>(services.GetRequiredService<IWorkspaceService>);
    }

    // TODO: Add constraint, so workspaces without users get deleted
    //  (private workspaces and shared of a deleted users)
    [Transactional]
    public async Task<UserEntity> CreateUser(UserEntity entity)
    {
        var user = await _userRepository.Create(entity);
        await _accountRepository.Create(new AccountEntity
        {
            UserId = user.Id,
        });
        await _workspaceService.Value.CreatePrivateUserWorkspace(user.Id);

        return user;
    }

    public static UserResponseDto BuildUserResponseDto(UserEntity entity)
    {
        return new UserResponseDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName
        };
    }

    public static UserSelfResponseDto BuildSelfUserResponseDto(UserEntity entity)
    {
        return new UserSelfResponseDto
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            SubscriptionKind = entity.SubscriptionKind?.ToEnum()
        };
    }
}