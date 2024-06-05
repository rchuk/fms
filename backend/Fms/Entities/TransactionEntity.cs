using System.ComponentModel.DataAnnotations;

namespace Fms.Entities;

public class TransactionEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int WorkspaceId { get; set; }
    [Required]
    public virtual WorkspaceEntity Workspace { get; set; } = null!;
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public virtual TransactionCategoryEntity Category { get; set; } = null!;
    
    [Required]
    public int Amount { get; set; }
    [MaxLength(2048)]
    public string? Description { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public DateTime CreationTimestamp { get; set; }
    [Required]
    public int CreationUserId { get; set; }
    [Required]
    public virtual UserEntity CreationUser { get; set; } = null!;
    
    public int? UserId { get; set; }
    public virtual UserEntity? User { get; set; }
}