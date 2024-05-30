using Fms.Entities;

namespace Fms.Repositories;

public interface IUserRepository
{
    Task<int> Create(UserEntity entity);
    Task<UserEntity?> Read(int id);
    Task<bool> Update(UserEntity entity);
    Task<bool> Delete(int id);
    Task<UserEntity?> FindByEmail(string email);
}