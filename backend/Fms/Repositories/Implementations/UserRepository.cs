using Fms.Application;
using Fms.Dtos;
using Fms.Entities;
using Fms.Entities.Common;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class UserRepository: BaseCrudRepository<UserEntity, int>, IUserRepository
{
    public UserRepository(FmsDbContext ctx) : base(ctx) {}

    public async Task<UserEntity?> FindByEmail(string email)
    {
        return await Ctx.Users.FirstOrDefaultAsync(entity => entity.Email == email);
    }

    public async Task<(int total, List<UserEntity> items)> List(UserCriteriaDto criteria, Pagination pagination)
    {
        var query = Ctx.Users.AsQueryable();
        if (criteria.Query is { } searchQuery)
        {
            query = query.Where(user => user.FirstName.ToLower().Contains(searchQuery.ToLower()) 
                                        || user.LastName.ToLower().Contains(searchQuery.ToLower())
                                        || user.Email.ToLower().Contains(searchQuery.ToLower()));
        }

        query = query.OrderBy(user => user.Id);

        return (
            query.Count(),
            await query.Skip(pagination.Offset).Take(pagination.Limit).ToListAsync()
        );
    }
}