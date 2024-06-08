using Fms.Dtos;
using Fms.Entities.Enums;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Organization controller
/// </summary>
[Route("api/organizations")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class OrganizationController(
    IOrganizationService organizationService
) : ControllerBase
{
    /// <summary>
    /// Create new organization
    /// </summary>
    /// <response code="200">Organization id</response>
    [HttpPut(Name = "createOrganization")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrganization([FromBody] OrganizationUpsertRequestDto requestDto)
    {
        return Ok(await organizationService.CreateOrganization(requestDto));
    }

    /// <summary>
    /// Get organization
    /// </summary>
    /// <response code="200">Organization data</response>
    [HttpGet("{id:int}", Name = "getOrganization")]
    [ProducesResponseType(typeof(OrganizationResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrganization([FromRoute] int id)
    {
        return Ok(await organizationService.GetOrganization(id));
    }
    
    /// <summary>
    /// Update organization
    /// </summary>
    [HttpPost("{id:int}", Name = "updateOrganization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrganization([FromRoute] int id, [FromBody] OrganizationUpsertRequestDto requestDto)
    {
        await organizationService.UpdateOrganization(id, requestDto);

        return Ok();
    }
    
    /// <summary>
    /// Delete organization
    /// </summary>
    [HttpDelete("{id:int}", Name = "deleteOrganization")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrganization([FromRoute] int id)
    {
        await organizationService.DeleteOrganization(id);

        return Ok();
    }
    
    /// <summary>
    /// Get list of current user's organizations
    /// </summary>
    [HttpGet(Name = "listUserOrganizations")]
    [ProducesResponseType(typeof(OrganizationListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCurrentUserOrganizations([FromQuery] PaginationDto pagination)
    {
        return Ok(await organizationService.ListCurrentUserOrganizations(pagination));
    }

    /// <summary>
    /// Add user to organization
    /// </summary>
    [HttpPut("{organizationId:int}/users/{userId:int}", Name = "organizationAddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddUser([FromRoute] int organizationId, [FromRoute] int userId)
    {
        await organizationService.AddUser(organizationId, userId);

        return Ok();
    }

    /// <summary>
    /// Remove user from organization
    /// </summary>
    [HttpDelete("{organizationId:int}/users/{userId:int}", Name = "organizationRemoveUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveUser([FromRoute] int organizationId, [FromRoute] int userId)
    {
        await organizationService.RemoveUser(organizationId, userId);

        return Ok();
    }

    /// <summary>
    /// Get user role in organization
    /// </summary>
    [HttpGet("{organizationId:int}/users/{userId:int}", Name = "organizationGetUserRole")]
    [ProducesResponseType(typeof(OrganizationRole), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserRole([FromRoute] int organizationId, [FromRoute] int userId)
    {
        return Ok(await organizationService.GetUserRole(organizationId, userId));
    }

    /// <summary>
    /// Update user role in organization
    /// </summary>
    [HttpPost("{organizationId:int}/users/{userId:int}", Name = "organizationUpdateUserRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole([FromRoute] int organizationId, [FromRoute] int userId, [FromBody] OrganizationRole role)
    {
        await organizationService.UpdateUserRole(organizationId, userId, role);

        return Ok();
    }
    
    /// <summary>
    /// Get list of users in organization
    /// </summary>
    /// <response code="200">List of users</response>
    [HttpGet("{organizationId:int}/users", Name = "listOrganizationUsers")]
    [ProducesResponseType(typeof(OrganizationUserListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListOrganizationUsers([FromRoute] int organizationId, [FromQuery] PaginationDto pagination)
    {
        return Ok(await organizationService.ListOrganizationUsers(organizationId, pagination));
    }
}
