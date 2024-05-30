using System.ComponentModel.DataAnnotations;
using Messages = Fms.Localization.Dtos.UserRegisterRequestDto;

namespace Fms.Dtos;

public class UserRegisterRequestDto
{
    /// <example>meowmeow@gmail.com</example>
    [MinLength(3, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "email_short")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "email_long")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "email_empty")]
    public required string Email { get; set; }
    /// <example>password1</example>
    [MinLength(8, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "password_short")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "password_long")]
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "password_empty")]
    public required string Password { get; set; }
}