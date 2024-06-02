using Fms.Dtos;
using Fms.Dtos.Common;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Entities.Enums;

namespace Fms.Services;

public interface IOrganizationService
{
    public Task<int> CreateOrganization(OrganizationUpsertRequestDto request);
    public Task<OrganizationResponseDto> GetOrganization(int id);
    public Task UpdateOrganization(int id, OrganizationUpsertRequestDto request);
    public Task DeleteOrganization(int id);

    public Task AddUser(int organizationId, int userId);
    public Task RemoveUser(int organizationId, int userId);
    public Task<OrganizationRole?> GetUserRole(int organizationId, int userId);
    public Task UpdateUserRole(int organizationId, int userId, OrganizationRole role);

    public Task<OrganizationUserListResponseDto> ListOrganizationUsers(int id, Pagination pagination);
    public Task<OrganizationListResponseDto> ListCurrentUserOrganizations(Pagination pagination);
}