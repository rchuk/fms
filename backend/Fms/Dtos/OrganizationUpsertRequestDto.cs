using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class OrganizationUpsertRequestDto
{
    /// <example>New Organization Name</example>
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string Name { get; set; }
}