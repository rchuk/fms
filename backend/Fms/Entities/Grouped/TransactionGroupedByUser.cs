using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByUser
{
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<TransactionEntity>? History { get; set; }
}

public class TransactionGroupedByUserList
{
    [Required]
    public required int TotalAmount { get; set; }
    [Required]
    public required List<TransactionGroupedByUser> Items { get; set; }
}
