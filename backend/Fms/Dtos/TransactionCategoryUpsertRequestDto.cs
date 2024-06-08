using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class TransactionCategoryUpsertRequestDto
{
    /// <example>Food</example>
    [Required]
    [MinLength(3)]
    [MaxLength(255)]
    public required string Name { get; set; }
    [Required]
    public required TransactionCategoryKind Kind { get; set; }
    /// <example>FF7E38</example>
    [Length(6, 6)]
    public string? UiColor { get; set; }
}