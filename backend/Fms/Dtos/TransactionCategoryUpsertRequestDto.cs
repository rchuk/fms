using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class TransactionCategoryUpsertRequestDto
{
    [Required, MaxLength(255)]
    public required string Name { get; set; }
    [Required]
    public required TransactionCategoryKind Kind { get; set; }
    [Length(6, 6)]
    public string? UiColor { get; set; }
}