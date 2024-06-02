using Fms.Entities;

namespace Fms.Repositories;

public interface IOrganizationRepository
{
    Task<int> Create(OrganizationEntity entity);
    Task<OrganizationEntity?> Read(int id);
    Task<bool> Update(OrganizationEntity entity);
    Task<bool> Delete(int id);
}