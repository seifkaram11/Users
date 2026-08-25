using eCommerce.Core.DTOs;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;
namespace eCommerce.Controller;

[ApiController]
[Route("api/1/Auth")]
public class AuthController:ControllerBase
{
    IUsersService _usersService;

    public AuthController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        if(request is null)return BadRequest("Invalid request");

        var res=await _usersService.LoginAsync(request);

        if (res is null)return ValidationProblem();

        return Ok(res);
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterRequest registerRequest)
    {
        if(registerRequest is null)return BadRequest("Invalid request");

        var res=await _usersService.RegisterAsync(registerRequest);

        if(res is null)return BadRequest();

        return Ok(res);
    }

    [HttpPost("RefreshTokensAsync")]
    public async Task<ActionResult> RefreshTokensAsync(RefreshTokenRequestDto request)
    {
        if(request is null)return BadRequest("Invalid request");

        var res=await _usersService.RefreshTokensAsync(request);

        if (res is null)return ValidationProblem();

        return Ok(res);
    }
}
