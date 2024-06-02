using Fms.Entities;

namespace Fms.Repositories;

public interface IOrganizationRepository
{
    Task<OrganizationEntity> Create(OrganizationEntity entity);
    Task<OrganizationEntity?> Read(int id);
    Task<bool> Update(OrganizationEntity entity);
    Task<bool> Delete(int id);
    
    Task<OrganizationEntity?> FindByName(string name);
}