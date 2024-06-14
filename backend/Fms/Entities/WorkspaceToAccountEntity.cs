using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Fms.Entities;

[PrimaryKey(nameof(WorkspaceId), nameof(AccountId))]
public class WorkspaceToAccountEntity
{
    [Required]
    public int WorkspaceId { get; set; }
    [Required]
    public virtual WorkspaceEntity Workspace { get; set; } = null!;
    [Required]
    public int AccountId { get; set; }
    [Required]
    public AccountEntity Account { get; set; } = null!;
    
    [Required]
    public int RoleId { get; set; }
    [Required]
    public WorkspaceRoleEntity Role { get; set; } = null!;
}