using Fms.Dtos;
using Fms.Entities.Common;

namespace Fms.Services;

public interface ITransactionCategoryService
{
    public Task<int> CreateUserTransactionCategory(TransactionCategoryUpsertRequestDto request);
    public Task<int> CreateOrganizationTransactionCategory(int organizationId, TransactionCategoryUpsertRequestDto request);
    public Task<int> CreateWorkspaceTransactionCategory(int workspaceId, TransactionCategoryUpsertRequestDto request);
    public Task<TransactionCategoryResponseDto> GetTransactionCategory(int id);
    public Task UpdateTransactionCategory(int id, TransactionCategoryUpsertRequestDto request);
    public Task DeleteTransactionCategory(int id);

    public Task<TransactionCategoryListResponseDto> ListUserTransactionCategories(PaginationDto pagination);
    public Task<TransactionCategoryListResponseDto> ListOrganizationTransactionCategories(int organizationId, PaginationDto pagination);
    public Task<TransactionCategoryListResponseDto> ListWorkspaceTransactionCategories(int workspaceId, PaginationDto pagination);
}