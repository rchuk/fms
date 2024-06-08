using System.ComponentModel.DataAnnotations;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class UserSelfResponseDto
{
    /// <example>25</example>
    [Required]
    public required int Id { get; set; }
    /// <example>John</example>
    [Required]
    public required string FirstName { get; set; }
    /// <example>Snow</example>
    [Required]
    public required string LastName { get; set; }
    /// <example>meow@gmail.com</example>
    [Required]
    public required string Email { get; set; }
    [Required]
    public required SubscriptionKind? SubscriptionKind { get; set; }
}