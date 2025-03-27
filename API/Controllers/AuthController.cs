using Application.Features.Auth.Commands;
using Application.Features.Auth.DTOs;
using Application.Features.Auth.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterDto registerDto)
    {
        return HandleResult(await Mediator.Send(new Register.Command { RegisterDto = registerDto }));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        return HandleResult(await Mediator.Send(new Login.Command { LoginDto = loginDto }));
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        return HandleResult(await Mediator.Send(new GetCurrentUser.Query()));
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        return HandleResult(await Mediator.Send(new Logout.Command()));
    }
}
