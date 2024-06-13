using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;

namespace Fms.Dtos;

public class UserResponseDto
{
    /// <example>25</example>
    [Required]
    public required int Id { get; set; }
    /// <example>John</example>
    [Required]
    public required string FirstName { get; set; }
    /// <example>Snow</example>
    [Required]
    public required string LastName { get; set; }
}

public class UserListResponseDto : ListResponseDto<UserResponseDto>;
