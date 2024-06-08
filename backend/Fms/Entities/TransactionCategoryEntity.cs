﻿using System.ComponentModel.DataAnnotations;

namespace Fms.Entities;

public class TransactionCategoryEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OwnerAccountId { get; set; }
    [Required]
    public AccountEntity OwnerAccount { get; set; } = null!;

    public int? WorkspaceId { get; set; }
    public virtual WorkspaceEntity? Workspace { get; set; }
    
    [Required, MaxLength(255)]
    public string Name { get; set; } = null!;
    [Required]
    public TransactionCategoryKindEntity Kind { get; set; } = null!;
    [Required, Length(6, 6)]
    public string UiColor { get; set; } = null!;
}