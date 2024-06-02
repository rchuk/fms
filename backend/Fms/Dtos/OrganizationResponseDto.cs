using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class OrganizationResponseDto
{
    /// <example>243</example>
    [Required]
    public required int Id { get; set; }
    /// <example>Cool Organization Inc.</example>
    [Required]
    public required string Name { get; set; }
}