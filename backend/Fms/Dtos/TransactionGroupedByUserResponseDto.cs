using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;

namespace Fms.Dtos;

public class TransactionGroupedByUserResponseDto
{
    [Required]
    public required UserResponseDto User { get; set; }
    
    public List<TransactionShortResponseDto>? History { get; set; }
    
    /// <example>-4320</example>
    [Required]
    public int Amount { get; set; }
}

public class TransactionGroupedByUserListResponseDto
{
    /// <example>0</example>
    [Required]
    public required int TotalAmount { get; set; }

    [Required]
    public required List<TransactionGroupedByUserResponseDto> Items { get; set; }
}
