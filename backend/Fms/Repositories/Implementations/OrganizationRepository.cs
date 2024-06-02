using Fms.Application;
using Fms.Entities;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class OrganizationRepository : BaseCrudRepository<OrganizationEntity, int>, IOrganizationRepository
{
    public OrganizationRepository(FmsDbContext ctx) : base(ctx) {}

    public async Task<OrganizationEntity?> FindByName(string name)
    {
        return await Ctx.Organizations.FirstOrDefaultAsync(entity => entity.Name == name);
    }
}