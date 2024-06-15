using Fms.Application.Attributes;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Exceptions;
using Fms.Repositories;

namespace Fms.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly Lazy<IWorkspaceService> _workspaceService;
    private readonly Lazy<IAuthService> _authService;
    private readonly Random _rng = new ();
    
    public UserService(
        IUserRepository userRepository,
        IAccountRepository accountRepository,
        IServiceProvider services
    )
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _workspaceService = new Lazy<IWorkspaceService>(services.GetRequiredService<IWorkspaceService>);
        _authService = new Lazy<IAuthService>(services.GetRequiredService<IAuthService>);
    }

    // TODO: Add constraint, so workspaces without users get deleted
    //  (private workspaces and shared of a deleted users)
    [Transactional]
    public async Task<UserEntity> CreateUser(UserEntity entity)
    {
        entity.FirstName = GetRandomFirstName();
        entity.LastName = GetRandomLastName();
        var user = await _userRepository.Create(entity);
        await _accountRepository.Create(new AccountEntity
        {
            UserId = user.Id
        });
        await _workspaceService.Value.CreatePrivateUserWorkspace(user.Id);

        return user;
    }
    
    [Transactional]
    public async Task UpdateUser(UserUpdateDto request)
    {
        var user = await _authService.Value.GetCurrentUser(); // TODO: Create merger
        if (request.FirstName is { } firstName)
            user.FirstName = firstName;
        if (request.LastName is { } lastName)
            user.LastName = lastName;

        if (!await _userRepository.Update(user))
            throw new PublicClientException();
    }

    [Transactional]
    public async Task<UserListResponseDto> ListUsers(UserCriteriaDto criteria, PaginationDto pagination)
    {
        var (total, items) = await _userRepository.List(criteria, new Pagination(pagination));

        return new UserListResponseDto
        {
            TotalCount = total,
            Items = items.Select(BuildUserResponseDto).ToList()
        };
    }

    [Transactional]
    public async Task<UserSelfResponseDto> GetCurrentUser()
    {
        var user = await _authService.Value.GetCurrentUser();
        var workspace = await _workspaceService.Value.GetCurrentUserPrivateWorkspace();

        var response = BuildSelfUserResponseDto(user);
        response.PrivateWorkspace = workspace;

        return response;
    }
    
    [Transactional]
    public async Task<UserResponseDto> GetUser(int id)
    {
        var user = await _userRepository.Read(id);
        if (user is null)
            throw new PublicNotFoundException();

        return BuildUserResponseDto(user);
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

    private string GetRandomFirstName()
    {
        return FirstNames[_rng.Next(FirstNames.Length)];
    }

    private string GetRandomLastName()
    {
        return LastNames[_rng.Next(LastNames.Length)];
    }
    
    private static readonly string[] FirstNames =
    [
        "Red",
        "Blue",
        "Green",
        "Yellow",
        "Purple",
        "Orange",
        "Pink",
        "Brown",
        "Black",
        "White",
        "Cyan",
        "Magenta",
        "Lime",
        "Maroon",
        "Navy",
        "Olive",
        "Teal",
        "Aqua",
        "Silver",
        "Gold"
    ];
    
    private static readonly string[] LastNames =
    [
        "Dog",
        "Cat",
        "Elephant",
        "Tiger",
        "Lion",
        "Giraffe",
        "Zebra",
        "Kangaroo",
        "Panda",
        "Koala",
        "Monkey",
        "Rabbit",
        "Deer",
        "Horse",
        "Dolphin",
        "Shark",
        "Eagle",
        "Penguin",
        "Turtle",
        "Owl"
    ];
}