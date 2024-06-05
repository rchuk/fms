using System.ComponentModel.DataAnnotations;
using Fms.Dtos.Common;
using Fms.Entities.Enums;

namespace Fms.Dtos;

public class TransactionCategoryResponseDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public required AccountResponseDto OwnerAccount { get; set; }
    [Required]
    public TransactionCategoryKind Kind { get; set; }
    [Required]
    public string UiColor { get; set; }
    
    public WorkspaceShortResponseDto? Workspace { get; set; }
}

public class TransactionCategoryListResponseDto : ListResponseDto<TransactionCategoryResponseDto>;
