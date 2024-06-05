using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class WorkspaceResponseDto
{
    /// <example>3501</example>
    [Required]
    public int Id { get; set; }
    /// <example>Family workspace</example>
    [Required]
    public required string Name { get; set; }
    [Required]
    public required WorkspaceKind Kind { get; set; }
    [Required]
    public required WorkspaceRole Role { get; set; }
}

public class WorkspaceListResponseDto : ListResponseDto<WorkspaceResponseDto>;
