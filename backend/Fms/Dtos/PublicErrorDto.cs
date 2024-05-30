using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class PublicErrorDto
{
    /// <example>Something bad happened</example>
    [Required]
    public required string Description { get; set; }
}