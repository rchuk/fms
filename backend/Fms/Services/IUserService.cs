using Fms.Dtos;
using Fms.Entities;

namespace Fms.Services;

public interface IUserService
{
    public Task<UserEntity> CreateUser(UserEntity entity);
    public Task UpdateUser(UserUpdateDto request);
}