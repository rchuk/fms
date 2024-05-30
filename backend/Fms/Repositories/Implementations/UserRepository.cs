using Fms.Application;
using Fms.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fms.Repositories.Implementations;

public class UserRepository: BaseRepository<UserEntity, int>, IUserRepository
{
    public UserRepository(FmsDbContext ctx) : base(ctx, entity => entity.Id)
    {}

    public async Task<UserEntity?> FindByEmail(string email)
    {
        return await Ctx.Users.FirstOrDefaultAsync(entity => entity.Email == email);
    }
}