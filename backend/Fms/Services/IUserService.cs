using Fms.Entities;

namespace Fms.Services;

public interface IUserService
{
    public Task<UserEntity> CreateUser(UserEntity entity);
}