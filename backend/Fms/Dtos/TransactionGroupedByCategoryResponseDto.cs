using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;

namespace Fms.Dtos;

public class TransactionGroupedByCategoryResponseDto
{
    [Required]
    public required TransactionCategoryResponseDto Category { get; set; }
    
    /// <example>15320</example>
    [Required]
    public int Amount { get; set; }
}
