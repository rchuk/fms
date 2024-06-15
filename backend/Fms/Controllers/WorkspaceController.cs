using Fms.Dtos;
using Fms.Entities.Common;
using Fms.Entities.Enums;
using Fms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fms.Controllers;

/// <summary>
/// Workspace controller
/// </summary>
[Route("api/workspaces")]
[ApiController]
[Authorize]
[Produces("application/json")]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status500InternalServerError)]
public class WorkspaceController(
    IWorkspaceService workspaceService
) : ControllerBase
{
    /// <summary>
    /// Create shared user workspace
    /// </summary>
    [HttpPut(Name = "createSharedUserWorkspace")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSharedUserWorkspace([FromBody] WorkspaceUpsertRequestDto requestDto)
    {
        return Ok(await workspaceService.CreateSharedUserWorkspace(requestDto));
    }
    
    /// <summary>
    /// Create organization workspace
    /// </summary>
    [HttpPut("/api/organizations/{organizationId:int}/workspaces", Name = "createOrganizationWorkspace")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrganizationWorkspace([FromRoute] int organizationId, [FromBody] WorkspaceUpsertRequestDto requestDto)
    {
        return Ok(await workspaceService.CreateSharedOrganizationWorkspace(organizationId, requestDto));
    }

    /// <summary>
    /// Get workspace
    /// </summary>
    [HttpGet("{id:int}", Name = "getWorkspace")]
    [ProducesResponseType(typeof(WorkspaceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkspace([FromRoute] int id)
    {
        return Ok(await workspaceService.GetWorkspace(id));
    }
    
    /// <summary>
    /// Update workspace
    /// </summary>
    [HttpPost("{id:int}", Name = "updateWorkspace")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWorkspace([FromRoute] int id,
        [FromBody] WorkspaceUpsertRequestDto requestDto)
    {
        await workspaceService.UpdateWorkspace(id, requestDto);

        return Ok();
    }

    /// <summary>
    /// Delete workspace
    /// </summary>
    [HttpDelete("{id:int}", Name = "deleteWorkspace")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWorkspace([FromRoute] int id)
    {
        await workspaceService.DeleteWorkspace(id);

        return Ok();
    }

    /// <summary>
    /// Get list of current user's workspaces
    /// </summary>
    [HttpGet(Name = "listUserWorkspaces")]
    [ProducesResponseType(typeof(WorkspaceListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ListCurrentUserWorkspaces([FromQuery] PaginationDto pagination)
    {
        return Ok(await workspaceService.ListCurrentUserWorkspaces(pagination));
    }
    
    /// <summary>
    /// Get list of organization's workspaces
    /// </summary>
    [HttpGet("/api/organizations/{organizationId:int}/workspaces", Name = "listOrganizationWorkspaces")]
    [ProducesResponseType(typeof(WorkspaceListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListOrganizationWorkspaces([FromRoute] int organizationId, [FromQuery] PaginationDto pagination)
    {
        return Ok(await workspaceService.ListOrganizationWorkspaces(organizationId, pagination));
    }

    /// <summary>
    /// Add user to workspace
    /// </summary>
    [HttpPut("{workspaceId:int}/users/{userId:int}", Name = "workspaceAddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddUser([FromRoute] int workspaceId, [FromRoute] int userId)
    {
        await workspaceService.AddUser(workspaceId, userId);

        return Ok();
    }
    
    /// <summary>
    /// Remove user from workspace
    /// </summary>
    [HttpDelete("{workspaceId:int}/users/{userId:int}", Name = "workspaceRemoveUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveUser([FromRoute] int workspaceId, [FromRoute] int userId)
    {
        await workspaceService.RemoveUser(workspaceId, userId);

        return Ok();
    }

    /// <summary>
    /// Get user role in workspace
    /// </summary>
    [HttpGet("{workspaceId:int}/users/{userId:int}", Name = "workspaceGetUserRole")]
    [ProducesResponseType(typeof(WorkspaceRole), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserRole([FromRoute] int workspaceId, [FromRoute] int userId)
    {
        return Ok(await workspaceService.GetUserRole(workspaceId, userId));
    }

    /// <summary>
    /// Update user role in workspace
    /// </summary>
    [HttpPost("{workspaceId:int}/users/{userId:int}", Name = "workspaceUpdateUserRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUserRole([FromRoute] int workspaceId, [FromRoute] int userId,
        [FromBody] WorkspaceRole role)
    {
        await workspaceService.UpdateUserRole(workspaceId, userId, role);

        return Ok();
    }
    
    /// <summary>
    /// Get list of users in workspace
    /// </summary>
    [HttpGet("{workspaceId:int}/users", Name = "listWorkspaceUsers")]
    [ProducesResponseType(typeof(WorkspaceUserListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PublicClientErrorDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PublicErrorDto), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListWorkspaceUsers([FromRoute] int workspaceId, [FromQuery] UserCriteriaDto criteria,
        [FromQuery] PaginationDto pagination)
    {
        return Ok(await workspaceService.ListWorkspaceUsers(workspaceId, criteria, pagination));
    }
}