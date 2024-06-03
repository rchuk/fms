using Fms.Entities;

namespace Fms.Repositories;

public interface IAccountRepository
{
    Task<AccountEntity> Create(AccountEntity entity);
    Task<AccountEntity?> Read(int id);
}