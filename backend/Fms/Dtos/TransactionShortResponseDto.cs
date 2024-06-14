using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class TransactionShortResponseDto
{
    /// <example>2138</example>
    [Required]
    public int Id { get; set; }
    /// <example>120</example>
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime Timestamp { get; set; }
}