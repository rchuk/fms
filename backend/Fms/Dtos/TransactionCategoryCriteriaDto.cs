namespace Fms.Dtos;

public class TransactionCategoryCriteriaDto
{
    public bool? IncludeOwner { get; set; }
    
    public string? Query { get; set; }
}