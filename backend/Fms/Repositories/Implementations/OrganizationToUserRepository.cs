﻿using Fms.Application;
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
            .OrderBy(map => map.UserId)
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
            .OrderBy(map => map.OrganizationId)
            .Include(map => map.Organization);
            
        return (
            query.Count(), 
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }

    public async Task<bool> AreRelatedUsers(int userIdFirst, int userIdSecond)
    {
        if (userIdFirst == userIdSecond)
            return true;

        return await Ctx.OrganizationToUser
            .Where(map => map.UserId == userIdFirst)
            .Select(map => map.OrganizationId)
            .Intersect(
                Ctx.OrganizationToUser
                    .Where(map => map.UserId == userIdSecond)
                    .Select(map => map.OrganizationId)
            )
            .AnyAsync();
    }
}