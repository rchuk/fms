using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class TransactionUpsertRequestDto
{
    /// <example>6402</example>
    [Required]
    public int CategoryId { get; set; }
    /// <example>640</example>
    [Required]
    public int Amount { get; set; }
    /// <example>Some different optional description</example>
    [MaxLength(2048)]
    public string? Description { get; set; }
    
    /// <example>null</example>
    public DateTime? Timestamp { get; set; }
    /// <example>null</example>
    public int? UserId { get; set; }
}