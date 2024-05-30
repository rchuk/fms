using Fms.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fms.Application;

public class FmsDbContext(DbContextOptions<FmsDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; } = null!;
}