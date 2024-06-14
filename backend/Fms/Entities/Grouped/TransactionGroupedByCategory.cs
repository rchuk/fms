using System.ComponentModel.DataAnnotations;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByCategory
{
    [Required]
    public required TransactionCategoryEntity Category { get; set; }
    [Required]
    public required int Amount { get; set; }
}