using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;

namespace Fms.Dtos;

public class TransactionGroupedByCategoryResponseDto
{
    [Required]
    public required TransactionCategoryResponseDto Category { get; set; }
    
    public List<TransactionShortResponseDto>? History { get; set; }
    
    /// <example>15320</example>
    [Required]
    public int Amount { get; set; }
}

public class TransactionGroupedByCategoryListResponseDto
{
    /// <example>-340</example>
    [Required]
    public required int TotalAmount { get; set; }

    [Required]
    public required List<TransactionGroupedByCategoryResponseDto> Items { get; set; }
}
