using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;

namespace Fms.Repositories;

public interface IUserRepository
{
    Task<UserEntity> Create(UserEntity entity);
    Task<UserEntity?> Read(int id);
    Task<bool> Update(UserEntity entity);
    Task<bool> Delete(int id);
    Task<UserEntity?> FindByEmail(string email);

    Task<(int total, List<UserEntity> items)> List(UserCriteriaDto criteria, Pagination pagination);
}