using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class WorkspaceShortResponseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public required WorkspaceKind Kind { get; set; }
}