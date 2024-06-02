using Fms.Dtos;
using Fms.Entities.Enums;

namespace Fms.Services;

public interface IOrganizationService
{
    public Task<int> CreateOrganization(OrganizationUpsertRequestDto request);
    public Task<OrganizationResponseDto> GetOrganization(int id);
    public Task<bool> UpdateOrganization(int id, OrganizationUpsertRequestDto request);
    public Task<bool> DeleteOrganization(int id);
    
    public Task AddUser(int organizationId, int userId);
    public Task RemoveUser(int organizationId, int userId);
    public Task<OrganizationRole?> GetUserRole(int organizationId, int userId);
    public Task UpdateUserRole(int organizationId, int userId, OrganizationRole role);
}