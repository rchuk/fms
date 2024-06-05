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
       
    [Required]
    public int Amount { get; set; }
    public string? Description { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; }
    [Required]
    public DateTime CreationTimestamp { get; set; }
    [Required]
    public required UserResponseDto CreationUser { get; set; }
    
    public UserResponseDto? User { get; set; }
}
