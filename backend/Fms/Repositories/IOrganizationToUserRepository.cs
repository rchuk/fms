using Fms.Entities;
using Fms.Entities.Common;

namespace Fms.Repositories;

public interface IOrganizationToUserRepository
{
    Task<OrganizationToUserEntity> Create(OrganizationToUserEntity entity);
    Task<OrganizationToUserEntity?> Read((int organizationId, int userId) id);
    Task<bool> Update(OrganizationToUserEntity entity);
    Task<bool> Delete((int organizationId, int userId) id);
    
    Task<(int total, IEnumerable<OrganizationToUserEntity> items)> ListOrganizationUsers(int organizationId, Pagination pagination);
    Task<(int total, IEnumerable<OrganizationToUserEntity> items)> ListUserOrganizations(int userId, Pagination pagination);
}