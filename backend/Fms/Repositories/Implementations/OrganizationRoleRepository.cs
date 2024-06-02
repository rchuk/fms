using Fms.Application;
using Fms.Entities;
using Fms.Entities.Enums;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class OrganizationRoleRepository : BaseEnumRepository<OrganizationRoleEntity, OrganizationRole>
{
    public OrganizationRoleRepository(FmsDbContext ctx) : base(ctx) {}
}