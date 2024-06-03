using System.ComponentModel.DataAnnotations;

namespace Fms.Entities;

public class AccountEntity
{
    [Key]
    public int Id { get; set; }
    public virtual UserEntity? User { get; set; }
    public int? UserId { get; set; }
    public virtual OrganizationEntity? Organization { get; set; }
    public int? OrganizationId { get; set; }
}