using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;

namespace Fms.Dtos;

public class TransactionGroupedByUserResponseDto
{
    [Required]
    public required UserResponseDto User { get; set; }
    
    /// <example>-4320</example>
    [Required]
    public int Amount { get; set; }
}
