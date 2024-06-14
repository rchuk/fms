using System.ComponentModel.DataAnnotations;

namespace Fms.Entities.Grouped;

public class TransactionGroupedByUser
{
    [Required]
    public UserEntity User { get; set; }
    
    [Required]
    public int Amount { get; set; }
}