using System.ComponentModel.DataAnnotations;

namespace Fms.Entities;

public class WorkspaceEntity
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;
    [Required]
    public WorkspaceKindEntity Kind { get; set; } = null!;

    public virtual List<WorkspaceToAccountEntity> Accounts { get; set; } = null!;
}