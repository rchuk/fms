using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class OrganizationUpsertRequestDto
{
    /// <example>New Organization Name</example>
    [Required]
    public required string Name { get; set; }
}