using System.ComponentModel.DataAnnotations;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByUser
{
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public int Amount { get; set; }
}

public class TransactionGroupedByUserList
{
    [Required]
    public required int TotalAmount { get; set; }
    [Required]
    public required List<TransactionGroupedByUser> Items { get; set; }
}
