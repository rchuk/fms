using Fms.Dtos;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Transaction category controller
/// </summary>
[Route("api/transactions/categories")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class TransactionCategoryController(
    ITransactionCategoryService transactionCategoryService
) : Controller
{
    /// <summary>
    /// Create user transaction category
    /// </summary>
    [HttpPut(Name = "createUserTransactionCategory")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserTransactionCategory([FromBody] TransactionCategoryUpsertRequestDto requestDto)
    {
        return Ok(await transactionCategoryService.CreateUserTransactionCategory(requestDto));
    }
    
    /// <summary>
    /// Create organization transaction category
    /// </summary>
    [HttpPut("/api/organizations/{organizationId:int}/transactions/categories", Name = "createOrganizationTransactionCategory")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrganizationTransactionCategory([FromRoute] int organizationId,
        [FromBody] TransactionCategoryUpsertRequestDto requestDto)
    {
        return Ok(await transactionCategoryService.CreateOrganizationTransactionCategory(organizationId, requestDto));
    }

    
    /// <summary>
    /// Create workspace transaction category
    /// </summary>
    [HttpPut("/api/workspaces/{workspaceId:int}/transactions/categories", Name = "createWorkspaceTransactionCategory")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateWorkspaceTransactionCategory([FromRoute] int workspaceId, [FromBody] TransactionCategoryUpsertRequestDto requestDto)
    {
        return Ok(await transactionCategoryService.CreateWorkspaceTransactionCategory(workspaceId, requestDto));
    }

    /// <summary>
    /// Get transaction category
    /// </summary>
    [HttpGet("{id:int}", Name = "getTransactionCategory")]
    [ProducesResponseType(typeof(WorkspaceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactionCategory([FromRoute] int id)
    {
        return Ok(await transactionCategoryService.GetTransactionCategory(id));
    }

    /// <summary>
    /// Update transaction category
    /// </summary>
    [HttpPost("{id:int}", Name = "updateTransactionCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransactionCategory([FromRoute] int id, [FromBody] TransactionCategoryUpsertRequestDto requestDto)
    {
        await transactionCategoryService.UpdateTransactionCategory(id, requestDto);

        return Ok();
    }

    /// <summary>
    /// Delete transaction category
    /// </summary>
    [HttpDelete("{id:int}", Name = "deleteTransactionCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransactionCategory([FromRoute] int id)
    {
        await transactionCategoryService.DeleteTransactionCategory(id);

        return Ok();
    }

    /// <summary>
    /// Get list of current user's transaction categories
    /// </summary>
    [HttpGet(Name = "listUserTransactionCategories")]
    [ProducesResponseType(typeof(TransactionCategoryListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCurrentUserTransactionCategories([FromQuery] PaginationDto pagination, [FromQuery] TransactionCategoryCriteriaDto criteriaDto)
    {
        return Ok(await transactionCategoryService.ListUserTransactionCategories(criteriaDto, pagination));
    }
    
    /// <summary>
    /// Get list of organization's transaction categories
    /// </summary>
    [HttpGet("/api/organizations/{organizationId:int}/transactions/categories", Name = "listOrganizationTransactionCategories")]
    [ProducesResponseType(typeof(TransactionCategoryListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListOrganizationTransactionCategories([FromRoute] int organizationId,
        [FromQuery] PaginationDto pagination, [FromQuery] TransactionCategoryCriteriaDto criteriaDto)
    {
        return Ok(await transactionCategoryService.ListOrganizationTransactionCategories(criteriaDto, organizationId, pagination));
    }
    
    /// <summary>
    /// Get list of workspace transaction categories
    /// </summary>
    // TODO: Add criteria to filter global ones vs workspace only
    [HttpGet("/api/workspaces/{workspaceId:int}/transactions/categories", Name = "listWorkspaceTransactionCategories")]
    [ProducesResponseType(typeof(TransactionCategoryListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListWorkspaceTransactionCategories([FromRoute] int workspaceId,
        [FromQuery] PaginationDto pagination, [FromQuery] TransactionCategoryCriteriaDto criteriaDto)
    {
        return Ok(await transactionCategoryService.ListWorkspaceTransactionCategories(criteriaDto, workspaceId, pagination));
    }
}
