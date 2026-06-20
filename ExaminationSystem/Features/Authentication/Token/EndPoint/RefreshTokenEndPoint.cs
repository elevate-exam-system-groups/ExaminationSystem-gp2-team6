using ExaminationSystem.Features.Authentication.Login.ViewModel;
using ExaminationSystem.Features.Authentication.Token.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.Token.EndPoint;

[ApiController]
[Route("api/auth")]
public class RefreshTokenEndPoint : ControllerBase
{
    private readonly IMediator _mediator;

    public RefreshTokenEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));

        if (!result.IsSuccess)
        {
            Response.Cookies.Delete("refreshToken");
            return Unauthorized(result.Message);
        }
        
        //  New refresh token in Cookie
        Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });
        
        return Ok(new LoginViewModel()
        {
            AccessToken = result.Data.AccessToken,
        });
    }
}