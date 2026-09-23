using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Application.Authentication.Common;
using OrderFlow.Application.Authentication.Login;
using OrderFlow.Application.Authentication.Logout;
using OrderFlow.Application.Authentication.Refresh;
using OrderFlow.Application.Authentication.Register;

namespace OrderFlow.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly RegisterService _registerService;
    private readonly LoginService _loginService;
    private readonly RefreshService _refreshService;
    private readonly LogoutService _logoutService;

    public AuthController(
        RegisterService registerService,
        LoginService loginService,
        RefreshService refreshService,
        LogoutService logoutService)
    {
        _registerService = registerService;
        _loginService = loginService;
        _refreshService = refreshService;
        _logoutService = logoutService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _registerService.ExecuteAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return Ok(await _loginService.ExecuteAsync(request, cancellationToken));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest request, CancellationToken cancellationToken)
    {
        var response = await _refreshService.ExecuteAsync(request, cancellationToken);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequest request, CancellationToken cancellationToken)
    {
        await _logoutService.ExecuteAsync(request, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            userId = User.FindFirst("sub")?.Value,
            email = User.FindFirst("email")?.Value,
            role = User.FindFirst(ClaimTypes.Role)?.Value
        });
    }
}
