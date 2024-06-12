namespace Fms.Entities.Criteria;

public class TransactionCategoryCriteria
{
    public int? AccountId { get; set; }
    public int? WorkspaceId { get; set; }
    
    public string? Query { get; set; }
}