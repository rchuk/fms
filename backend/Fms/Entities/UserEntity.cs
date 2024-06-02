using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Fms.Entities;

[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Required, Key]
    public int Id { get; set; }
    [Required, MaxLength(255)]
    public string Email { get; set; } = null!;
    [Required, MaxLength(344)]
    public string PasswordHash { get; set; } = null!;
    [Required, MaxLength(255)]
    public string FirstName { get; set; } = null!;
    [Required, MaxLength(255)]
    public string LastName { get; set; } = null!;

    public virtual List<OrganizationToUserEntity> Organizations { get; set; } = null!;
}