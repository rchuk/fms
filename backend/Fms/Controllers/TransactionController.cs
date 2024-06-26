﻿using Fms.Dtos;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Transaction controller
/// </summary>
[Route("api/transactions")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class TransactionController(
    ITransactionService transactionService
) : ControllerBase
{
    /// <summary>
    /// Create new transaction
    /// </summary>
    [HttpPut("/api/workspaces/{workspaceId:int}/transactions", Name = "createTransaction")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTransaction([FromRoute] int workspaceId,
        [FromBody] TransactionUpsertRequestDto requestDto)
    {
        return Ok(await transactionService.CreateTransaction(workspaceId, requestDto));
    }
    
    /// <summary>
    /// Get transaction
    /// </summary>
    [HttpGet("{id:int}", Name = "getTransaction")]
    [ProducesResponseType(typeof(TransactionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransaction([FromRoute] int id)
    {
        return Ok(await transactionService.GetTransaction(id));
    }
    
    /// <summary>
    /// Update transaction
    /// </summary>
    [HttpPost("{id:int}", Name = "updateTransaction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransaction([FromRoute] int id,
        [FromBody] TransactionUpsertRequestDto requestDto)
    {
        await transactionService.UpdateTransaction(id, requestDto);

        return Ok();
    }

    /// <summary>
    /// Delete transaction
    /// </summary>
    [HttpDelete("{id:int}", Name = "deleteTransaction")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
    {
        await transactionService.DeleteTransaction(id);

        return Ok();
    }
    
    /// <summary>
    /// Get list of workspace transactions
    /// </summary>
    [HttpGet("/api/workspaces/{workspaceId:int}/transactions", Name = "listTransactions")]
    [ProducesResponseType(typeof(TransactionListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListWorkspaceTransactions([FromRoute] int workspaceId,
        [FromQuery] TransactionCriteriaDto criteria, [FromQuery] PaginationDto pagination)
    {
        return Ok(await transactionService.ListWorkspaceTransactions(workspaceId, criteria, pagination));
    }
    
    /// <summary>
    /// Get list of workspace transactions grouped by category
    /// </summary>
    [HttpGet("/api/workspaces/{workspaceId:int}/transactions/group-by-category", Name = "listTransactionsGroupByCategory")]
    [ProducesResponseType(typeof(TransactionGroupedByCategoryListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListWorkspaceTransactionsGroupByCategory([FromRoute] int workspaceId,
        [FromQuery] TransactionCriteriaDto criteria)
    {
        return Ok(await transactionService.ListWorkspaceTransactionsGroupByCategory(workspaceId, criteria));
    }
    
    /// <summary>
    /// Get list of workspace transactions grouped by user
    /// </summary>
    [HttpGet("/api/workspaces/{workspaceId:int}/transactions/group-by-user", Name = "listTransactionsGroupByUser")]
    [ProducesResponseType(typeof(TransactionGroupedByUserListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListWorkspaceTransactionsGroupByUser([FromRoute] int workspaceId,
        [FromQuery] TransactionCriteriaDto criteria)
    {
        return Ok(await transactionService.ListWorkspaceTransactionsGroupByUser(workspaceId, criteria));
    }
}