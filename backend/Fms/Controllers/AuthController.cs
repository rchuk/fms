using Fms.Dtos;
using Fms.Services;
using Fms.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Authorization controller
/// </summary>
[Route("api/auth")]
[ApiController]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class AuthController(
    IAuthService authService
) : ControllerBase
{
    /// <summary>
    /// Attempts to register new user using credentials
    /// </summary>
    /// <response code="200">Returns access token</response>
    [HttpPost("register", Name = "register")]
    [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto requestDto)
    {
        return Ok(await authService.Register(requestDto));
    }

    /// <summary>
    /// Attempts to login using user credentials
    /// </summary>
    /// <response code="200">Returns access token</response>
    [HttpPost("login", Name = "login")]
    [Consumes("application/x-www-form-urlencoded")]
    [ProducesResponseType(typeof(AccessTokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromForm] UserLoginRequestDto requestDto)
    {
        return Ok(await authService.Login(requestDto));
    }

    /// <summary>
    /// Get id of the current user
    /// </summary>
    /// <response code="200">Returns id of the current user</response>
    [HttpGet("id", Name = "getId")]
    [Authorize]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUserId()
    {
        return Ok(await authService.GetCurrentUserId());
    }
    
    /// <summary>
    /// Get current user
    /// </summary>
    /// <response code="200">Returns id of the current user</response>
    [HttpGet("me", Name = "getMe")]
    [Authorize]
    [ProducesResponseType(typeof(UserSelfResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetCurrentUser()
    {
        return Ok(UserService.BuildSelfUserResponseDto(await authService.GetCurrentUser()));
    }
}
