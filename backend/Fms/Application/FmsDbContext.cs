using Fms.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fms.Application;

public class FmsDbContext(DbContextOptions<FmsDbContext> options) : DbContext(options)
{
    public DbSet<OrganizationEntity> Organizations { get; set; } = null!;
    public DbSet<OrganizationRoleEntity> OrganizationRoles { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<OrganizationToUserEntity>()
            .HasOne(map => map.Organization)
            .WithMany(organization => organization.Users);

        modelBuilder.Entity<OrganizationToUserEntity>()
            .HasOne(map => map.User)
            .WithMany(user => user.Organizations);
    }
}