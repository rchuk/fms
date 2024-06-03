using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fms.Entities;

[Index(nameof(Name), IsUnique = true)]
public class OrganizationEntity
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;

    public AccountEntity Account { get; set; } = null!;
    public virtual List<OrganizationToUserEntity> Users { get; set; } = null!;
}