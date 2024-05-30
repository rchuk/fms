using System.Text.Json.Serialization;

namespace Fms.Dtos;

public class PublicClientErrorDto : PublicErrorDto
{
    /// <example>
    ///     [
    ///         "Email can't contain less than 3 characters",
    ///         "Password can't be empty"
    ///     ]
    /// </example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ValidationErrors { get; set; }
}