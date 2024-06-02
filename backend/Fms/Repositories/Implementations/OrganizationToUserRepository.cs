using Fms.Application;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class OrganizationToUserRepository : BaseCrudRepository<OrganizationToUserEntity, (int organizationId, int userId)>, IOrganizationToUserRepository
{
    public OrganizationToUserRepository(FmsDbContext ctx) : base(ctx) {}
    
    public async Task<(int total, IEnumerable<OrganizationToUserEntity> items)> ListOrganizationUsers(int organizationId, Pagination pagination)
    {
        var query = Ctx.OrganizationToUser
            .Where(map => map.OrganizationId == organizationId)
            .OrderBy(map => map.User.Id)
            .Include(map => map.User);
            
        return (
            query.Count(), 
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }

    public async Task<(int total, IEnumerable<OrganizationToUserEntity> items)> ListUserOrganizations(int userId, Pagination pagination)
    {
        var query= Ctx.OrganizationToUser
            .Where(map => map.UserId == userId)
            .OrderBy(map => map.Organization.Id)
            .Include(map => map.Organization);
            
        return (
            query.Count(), 
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
}