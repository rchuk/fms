using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class WorkspaceUpsertRequestDto
{
    /// <example>Cool Descriptive Workspace Name</example>
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string Name { get; set; } = null!;
}