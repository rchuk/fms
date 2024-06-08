using Fms.Dtos;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Subscription controller
/// </summary>
[Route("api/subscriptions")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class SubscriptionController(
    ISubscriptionService subscriptionService
) : ControllerBase
{
    /// <summary>
    /// Buy subscription
    /// </summary>
    [HttpPost("buy", Name = "buySubscription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuySubscription([FromBody] BuySubscriptionRequestDto request)
    {
        await subscriptionService.BuySubscription(request);

        return Ok();
    }
}