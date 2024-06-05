using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos;

public class TransactionResponseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public required WorkspaceShortResponseDto Workspace { get; set; }
    [Required]
    public required TransactionCategoryResponseDto Category { get; set; }
    
    /// <example>-530</example>
    [Required]
    public int Amount { get; set; }
    /// <example>Some optional description</example>
    public string? Description { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public DateTime CreationTimestamp { get; set; }
    [Required]
    public required UserResponseDto CreationUser { get; set; }
    
    public UserResponseDto? User { get; set; }
}
