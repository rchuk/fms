using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class WorkspaceResponseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required WorkspaceKindEntity Kind { get; set; }
    [Required]
    public required WorkspaceRole Role { get; set; }
}

public class WorkspaceListResponseDto : ListResponseDto<WorkspaceResponseDto>;
