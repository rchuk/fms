using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities;
using Fms.Repositories;

namespace Fms.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;

    public UserService(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    [Transactional]
    public async Task<UserEntity> CreateUser(UserEntity entity)
    {
        var user = await _userRepository.Create(entity);
        await _accountRepository.Create(new AccountEntity
        {
            UserId = user.Id
        });

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
}