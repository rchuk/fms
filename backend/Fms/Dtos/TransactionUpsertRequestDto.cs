using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class TransactionUpsertRequestDto
{
    [Required]
    public int CategoryId { get; set; }
    [Required]
    public int Amount { get; set; }
    [MaxLength(2048)]
    public string? Description { get; set; }
    
    public DateTime? Timestamp { get; set; }
    public int? UserId { get; set; }
}