using System.ComponentModel.DataAnnotations;

namespace Fms.Dtos.Common;

public class ListResponseDto<T>
{
    /// <example>165</example>
    [Required]
    public int TotalCount { get; set; }
    [Required]
    public List<T> Items { get; set; } = [];
}