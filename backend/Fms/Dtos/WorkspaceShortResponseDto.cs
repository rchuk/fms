using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class WorkspaceShortResponseDto
{
    /// <example>1402</example>
    [Required]
    public int Id { get; set; }
    /// <example>Family Workspace</example>
    [Required]
    public required string Name { get; set; }
    [Required]
    public required WorkspaceKind Kind { get; set; }
}