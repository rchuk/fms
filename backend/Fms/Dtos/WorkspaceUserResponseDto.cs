using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class WorkspaceUserResponseDto
{
    [Required]
    public required UserResponseDto User { get; set; }
    [Required]
    public required WorkspaceRole Role { get; set; }
}

public class WorkspaceUserListResponseDto : ListResponseDto<WorkspaceUserResponseDto>;
