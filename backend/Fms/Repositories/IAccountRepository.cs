using Fms.Entities;

namespace Fms.Repositories;

public interface IAccountRepository
{
    Task<AccountEntity> Create(AccountEntity entity);
    Task<AccountEntity?> Read(int id);

    Task<AccountEntity?> GetUserAccount(int userId);
    Task<AccountEntity?> GetOrganizationAccount(int organizationId);
}