using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Fms.Dtos;

public class AccessTokenResponseDto
{
    /// <example>abcdefg123</example>
    [Required]
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    /// <example>Bearer</example>
    [Required]
    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; } = "Bearer";
    /// <example>3600</example>
    [Required]
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; set; }
}