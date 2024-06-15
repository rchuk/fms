using Fms.Dtos;
using Fms.Entities;

namespace Fms.Services;

public interface IUserService
{
    public Task<UserEntity> CreateUser(UserEntity entity);
    public Task UpdateUser(UserUpdateDto request);
    public Task<UserSelfResponseDto> GetCurrentUser();
    public Task<UserResponseDto> GetUser(int id);

    public Task<UserListResponseDto> ListUsers(UserCriteriaDto criteria, PaginationDto pagination);
}