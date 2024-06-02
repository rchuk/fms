using Fms.Dtos;
using Fms.Entities.Enums;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Organization controller
/// </summary>
[Route("api/organization")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class OrganizationController(
    IOrganizationService organizationService
) : ControllerBase
{
    [HttpPut]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] OrganizationUpsertRequestDto requestDto)
    {
        return Ok(await organizationService.CreateOrganization(requestDto));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrganizationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetOrganization([FromRoute] int id)
    {
        return Ok(await organizationService.GetOrganization(id));
    }
    
    // TODO: Don't return booleans?

    [HttpPost("{id:int}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateOrganization([FromRoute] int id, [FromBody] OrganizationUpsertRequestDto requestDto)
    {
        return Ok(await organizationService.UpdateOrganization(id, requestDto));
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteOrganization([FromRoute] int id)
    {
        return Ok(await organizationService.DeleteOrganization(id));
    }

    [HttpPut("{organizationId:int}/users/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task AddUser([FromRoute] int organizationId, [FromRoute] int userId)
    {
        await organizationService.AddUser(organizationId, userId);
    }

    [HttpDelete("{organizationId:int}/users/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task RemoveUser([FromRoute] int organizationId, [FromRoute] int userId)
    {
        await organizationService.RemoveUser(organizationId, userId);
    }

    [HttpGet("{organizationId:int}/users/{userId:int}")]
    [ProducesResponseType(typeof(OrganizationRole), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserRole([FromRoute] int organizationId, [FromRoute] int userId)
    {
        return Ok(await organizationService.GetUserRole(organizationId, userId));
    }

    // TODO: Create dto for role
    [HttpPost("{organizationId:int}/users/{userId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task UpdateUserRole([FromRoute] int organizationId, [FromRoute] int userId, [FromBody] OrganizationRole role)
    {
        await organizationService.UpdateUserRole(organizationId, userId, role);
    }
}