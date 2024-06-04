using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class WorkspaceUpsertRequestDto
{
    [Required]
    public required string Name { get; set; } = null!;
}