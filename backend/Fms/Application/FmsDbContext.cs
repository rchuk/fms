using Fms.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fms.Application;

public class FmsDbContext(DbContextOptions<FmsDbContext> options) : DbContext(options)
{
    public DbSet<OrganizationEntity> Organizations { get; set; } = null!;
    public DbSet<OrganizationRoleEntity> OrganizationRoles { get; set; } = null!;
    public DbSet<OrganizationToUserEntity> OrganizationToUser { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;

    public DbSet<AccountEntity> Accounts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        OnModelCreatingOrganizationToUser(modelBuilder);
        OnModelCreatingAccount(modelBuilder);
    }
    
    private void OnModelCreatingUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>()
            .Navigation(entity => entity.Account).AutoInclude();
    }

    private void OnModelCreatingOrganization(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizationEntity>()
            .Navigation(entity => entity.Account).AutoInclude();
    }
    
    private void OnModelCreatingOrganizationToUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrganizationToUserEntity>()
            .Navigation(map => map.Role).AutoInclude();
        
        modelBuilder.Entity<OrganizationToUserEntity>()
            .HasOne(map => map.Organization)
            .WithMany(organization => organization.Users)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrganizationToUserEntity>()
            .HasOne(map => map.User)
            .WithMany(user => user.Organizations)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void OnModelCreatingAccount(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>()
            .ToTable(t => t.HasCheckConstraint("one_of", "(\"UserId\" IS NULL) <> (\"OrganizationId\" IS NULL)"));

        modelBuilder.Entity<AccountEntity>()
            .HasOne(map => map.Organization)
            .WithOne(organization => organization.Account)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AccountEntity>()
            .HasOne(map => map.User)
            .WithOne(user => user.Account)
            .OnDelete(DeleteBehavior.Cascade);
    }
}