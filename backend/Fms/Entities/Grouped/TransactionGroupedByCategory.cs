using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByCategory
{
    [Required]
    public required TransactionCategoryEntity Category { get; set; }
    
    [Required]
    public required int Amount { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<TransactionEntity>? History { get; set; }
}

public class TransactionGroupedByCategoryList
{
    [Required]
    public required int TotalAmount { get; set; }
    [Required]
    public required List<TransactionGroupedByCategory> Items { get; set; }
}
