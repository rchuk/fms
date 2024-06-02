using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class OrganizationUserResponseDto
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
    [Required]
    public required OrganizationRole Role { get; set; }
}

public class OrganizationUserListResponseDto : ListResponseDto<OrganizationUserResponseDto>;
