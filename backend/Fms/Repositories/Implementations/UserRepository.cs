using Fms.Application;
using Fms.Entities;
using Fms.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class UserRepository: BaseCrudRepository<UserEntity, int>, IUserRepository
{
    public UserRepository(FmsDbContext ctx) : base(ctx, entity => entity.Id)
    {}

    public async Task<UserEntity?> FindByEmail(string email)
    {
        return await Ctx.Users.FirstOrDefaultAsync(entity => entity.Email == email);
    }
}