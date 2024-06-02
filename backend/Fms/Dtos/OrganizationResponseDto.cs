using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class OrganizationResponseDto
{
    /// <example>243</example>
    [Required]
    public required int Id { get; set; }
    /// <example>Cool Organization Inc.</example>
    [Required]
    public required string Name { get; set; }
    [Required]
    public required OrganizationRole Role { get; set; }
}

public class OrganizationListResponseDto : ListResponseDto<OrganizationResponseDto>;
