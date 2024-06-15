using System.ComponentModel.DataAnnotations;

namespace Fms.Entities.Grouped;

public class WorkspaceWithOwner
{
    [Required]
    public required WorkspaceToAccountEntity Map { get; set; }
    [Required]
    public required AccountEntity Owner { get; set; }
}