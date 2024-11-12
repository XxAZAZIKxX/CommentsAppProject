using CommentsApp.Api.Shared.Requests;
using CommentsApp.Api.Shared.Responses;
using CommentsApp.Api.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommentsApp.Api.Controllers;

[ApiController, Route("auth")]
public class AuthController : ControllerBase
{
    [HttpPost, Route("login")]
    public async Task<ActionResult<TokenResponse>> LoginAsync([FromServices] IUserService userService,
        [FromBody] LoginRequest request)
    {
        var result = await userService.LoginAsync(request);
        return result.Match(response => response, exception => throw exception);
    }

    [HttpPost, Route("register")]
    public async Task<ActionResult<TokenResponse>> RegisterAsync([FromServices] IUserService userService,
        [FromBody] RegisterRequest request)
    {
        var result = await userService.RegisterAsync(request);
        return result.Match(response => response, exception => throw exception);
    }
}