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

    public DbSet<WorkspaceEntity> Workspaces { get; set; } = null!;
    public DbSet<WorkspaceKindEntity> WorkspaceKinds { get; set; } = null!;
    public DbSet<WorkspaceRoleEntity> WorkspaceRoles { get; set; } = null!;
    public DbSet<WorkspaceToAccountEntity> WorkspaceToAccount { get; set; } = null!;

    public DbSet<TransactionCategoryEntity> TransactionCategories { get; set; } = null!;
    public DbSet<TransactionCategoryKindEntity> TransactionCategoryKinds { get; set; } = null!;

    public DbSet<TransactionEntity> Transactions { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        OnModelCreatingUser(modelBuilder);
        OnModelCreatingOrganization(modelBuilder);
        OnModelCreatingOrganizationToUser(modelBuilder);
        OnModelCreatingAccount(modelBuilder);
        OnModelCreatingWorkspace(modelBuilder);
        OnModelCreatingWorkspaceToAccount(modelBuilder);
        OnModelCreatingTransactionCategory(modelBuilder);
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

    private void OnModelCreatingWorkspace(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkspaceEntity>()
            .Navigation(entity => entity.Kind).AutoInclude();
    }
    
    private void OnModelCreatingWorkspaceToAccount(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkspaceToAccountEntity>()
            .Navigation(map => map.Account).AutoInclude();
        modelBuilder.Entity<WorkspaceToAccountEntity>()
            .Navigation(map => map.Role).AutoInclude();
        
        modelBuilder.Entity<WorkspaceToAccountEntity>()
            .HasOne(map => map.Workspace)
            .WithMany(workspace => workspace.Accounts)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkspaceToAccountEntity>()
            .HasOne(map => map.Account)
            .WithMany(account => account.Workspaces)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void OnModelCreatingTransactionCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionCategoryEntity>()
            .Navigation(entity => entity.Kind).AutoInclude();
        
        modelBuilder.Entity<TransactionCategoryEntity>()
            .Navigation(entity => entity.OwnerAccount).AutoInclude();
    }
}