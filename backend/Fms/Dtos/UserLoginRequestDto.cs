using System.ComponentModel.DataAnnotations;
using Messages = Fms.Localization.Dtos.UserLoginRequestDto;

namespace Fms.Dtos;

public class UserLoginRequestDto
{
    /// <example>meowmeow@gmail.com</example>
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "email_empty")]
    public required string Email { get; set; }
    /// <example>password1</example>
    [Required(ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "password_empty")]
    public required string Password { get; set; }
}
