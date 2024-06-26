﻿using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class OrganizationUserResponseDto
{
    [Required]
    public required UserResponseDto User { get; set; }
    [Required]
    public required OrganizationRole Role { get; set; }
}

public class OrganizationUserListResponseDto : ListResponseDto<OrganizationUserResponseDto>;
