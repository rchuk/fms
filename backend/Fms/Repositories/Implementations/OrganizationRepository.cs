using Fms.Application;
using Fms.Entities;
using Fms.Repositories.Common;

namespace Fms.Repositories.Implementations;

public class OrganizationRepository : BaseCrudRepository<OrganizationEntity, int>, IOrganizationRepository
{
    public OrganizationRepository(FmsDbContext ctx) : base(ctx, entity => entity.Id) {}
}