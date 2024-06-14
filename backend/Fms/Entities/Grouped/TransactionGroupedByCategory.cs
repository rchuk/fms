using System.ComponentModel.DataAnnotations;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByCategory
{
    [Required]
    public required TransactionCategoryEntity Category { get; set; }
    [Required]
    public required int Amount { get; set; }
}

public class TransactionGroupedByCategoryList
{
    [Required]
    public required int TotalAmount { get; set; }
    [Required]
    public required List<TransactionGroupedByCategory> Items { get; set; }
}
