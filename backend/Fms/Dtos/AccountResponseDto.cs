using System.Text.Json.Serialization;

namespace Fms.Dtos;

public class AccountResponseDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserResponseDto? User { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OrganizationShortResponseDto? Organization { get; set; }
}