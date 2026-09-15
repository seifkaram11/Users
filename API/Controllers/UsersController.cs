using Core.DTOs;
using Core.Entities;
using Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/V1/Users")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    IUsersService _usersService;
    IRolesService _rolesService;
    ILogger<UsersController> _logger;

    public UsersController(IUsersService usersService, ILogger<UsersController> logger, IRolesService rolesService)
    {
        _usersService = usersService;
        _logger = logger;
        _rolesService = rolesService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
    {
        var roles = await _usersService.GetAllUsersAsync();
        return Ok(roles);
    }

    [HttpPut("{UserId:guid}")]
    public async Task<ActionResult> UpdateUser(Guid UserId, [FromBody]UpdateUserRequest user)
    {
        var result = await _usersService.UpdateUserAsync(UserId, user);
        if (result) return Ok("User updated successfully.");
        else return BadRequest("Failed to update user.");
    }

    [HttpPut("Roles/{userId:guid}")]
    public async Task<ActionResult> UpdateUserRoles(Guid userId, [FromBody] IEnumerable<string> roleNames)
    {
        var result = await _rolesService.UpdateUserRolesAsync(userId, roleNames);
        if (result) return Ok("User roles updated successfully.");
        else return BadRequest("Failed to update user roles.");
    }

    [HttpPost()]
    public async Task<ActionResult> CreateUser([FromBody]AddUserRequest user)
    {
        var result = await _usersService.CreateUserAsync(user);
        if (result is not null) return Ok(result);
        else return BadRequest("Failed to create user.");
    }

    [HttpPost("Roles")]
    public async Task<ActionResult> AddRole([FromBody] AddRoleToUserRequest role)
    {
        var result = await _usersService.AddRoleAsync(role);
        if (result) return Ok("Role added successfully.");
        else return BadRequest("Failed to add role.");
    }

    [HttpDelete("{UserId:guid}")]
    public async Task<ActionResult> DeleteUser(Guid UserId)
    {
        var flag=await _usersService.DeleteUserAsync(UserId);
        if(!flag)return BadRequest("Failed to delete user.");
        return Ok("User deleted successfully.");
    }
}
