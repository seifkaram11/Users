using Core.Entities;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/V1/Roles")]
[Authorize(Roles = "Admin")]
public class RolesController : ControllerBase
{
    IRolesService _rolesService;
    ILogger<RolesController> _logger;

    public RolesController(IRolesService rolesService, ILogger<RolesController> logger)
    {
        _rolesService = rolesService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Roles>>> GetAllRoles()
    {
        var roles = await _rolesService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpPut("{roleId}")]
    public async Task<ActionResult> UpdateRole(Guid roleId,[FromBody]string newRoleName)
    {
        var result = await _rolesService.UpdateRole(roleId, newRoleName);
        if (result)
            return Ok("Role updated successfully.");
        else
            return BadRequest("Failed to update role.");
    }

    [HttpPost]
    public async Task<ActionResult> AddRole([FromBody]string role)
    {
        var result = await _rolesService.AddRoleAsync(role);
        if (result)
            return Ok("Role added successfully.");
        else
            return BadRequest("Failed to add role.");
    }
}
