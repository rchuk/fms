using Fms.Entities.Enums;

namespace Fms.Dtos;

public class TransactionCriteriaDto
{
    /// <example>35</example>
    public int? CategoryId { get; set; }
    /// <example>2024-06-07T07:00:00.000Z</example>
    public DateTime? StartDate { get; set; }
    /// <example>null</example>
    public DateTime? EndDate { get; set; }
    /// <example>null</example>
    public int? UserId { get; set; }
    /// <example>300</example>
    public int? MinAmount { get; set; }
    /// <example>null</example>
    public int? MaxAmount { get; set; }
    /// <example>null</example>
    public bool? IncludeHistory { get; set; }
    /// <example>null</example>
    public TransactionCategoryKind? CategoryKind { get; set; }
    
    public TransactionSortFieldDto? SortField { get; set; }
    public SortDirectionDto? SortDirection { get; set; }
    
    // TODO: Add search by name/description
}

public enum TransactionSortFieldDto
{
    Timestamp,
    Amount
}
