using Fms.Dtos;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// User controller
/// </summary>
[Route("api/users")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class UserController(
    IUserService userService
) : ControllerBase
{
    /// <summary>
    /// Update user profile
    /// </summary>
    [HttpPost(Name = "updateMe")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto request)
    {
        await userService.UpdateUser(request);

        return Ok();
    }
}