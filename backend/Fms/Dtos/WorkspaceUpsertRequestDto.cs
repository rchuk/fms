using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class WorkspaceUpsertRequestDto
{
    /// <example>Cool Descriptive Workspace Name</example>
    [Required]
    public required string Name { get; set; } = null!;
}