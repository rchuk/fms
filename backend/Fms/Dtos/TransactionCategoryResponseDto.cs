using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class TransactionCategoryResponseDto
{
    /// <example>5359</example>
    [Required]
    public int Id { get; set; }
    /// <example>Salary</example>
    [Required]
    public required string Name { get; set; }
    [Required]
    public required AccountResponseDto OwnerAccount { get; set; }
    [Required]
    public required TransactionCategoryKind Kind { get; set; }
    /// <example>FF0022</example>
    [Required]
    public required string UiColor { get; set; }
    
    public WorkspaceShortResponseDto? Workspace { get; set; }
}

public class TransactionCategoryListResponseDto : ListResponseDto<TransactionCategoryResponseDto>;
