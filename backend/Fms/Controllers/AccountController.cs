using Fms.Dtos;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Account controller
/// </summary>
[Route("api/accounts")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class AccountController(
    IAccountService accountService
) : ControllerBase
{
    /// <summary>
    /// Get account
    /// </summary>
    /// <response code="200">User or organization data</response>
    [HttpGet("{id:int}", Name = "getAccount")]
    [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount([FromRoute] int id)
    {
        return Ok(await accountService.GetAccount(id));
    }
}