using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Fms.Entities;

[PrimaryKey(nameof(OrganizationId), nameof(UserId))]
public class OrganizationToUserEntity
{
    [Required]
    public int OrganizationId { get; set; }
    [Required]
    public OrganizationEntity Organization { get; set; } = null!;
    [Required]
    public int UserId { get; set; }
    [Required]
    public UserEntity User { get; set; } = null!;
    [Required]
    public OrganizationRoleEntity Role { get; set; } = null!;
}