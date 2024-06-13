using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class UserUpdateDto
{
    /// <example>John</example>
    [MinLength(3)]
    public string? FirstName { get; set; }
    /// <example>Snow</example>
    [MinLength(3)]
    public string? LastName { get; set; }
}