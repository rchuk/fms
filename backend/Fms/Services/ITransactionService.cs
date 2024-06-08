using Fms.Dtos;

namespace Fms.Services;

public interface ITransactionService
{
    Task<int> CreateTransaction(int workspaceId, TransactionUpsertRequestDto requestDto);
    Task<TransactionResponseDto> GetTransaction(int id);
    Task UpdateTransaction(int id, TransactionUpsertRequestDto requestDto);
    Task DeleteTransaction(int id);

    Task<TransactionListResponseDto> ListWorkspaceTransactions(int workspaceId, TransactionCriteriaDto criteria, PaginationDto pagination);
}