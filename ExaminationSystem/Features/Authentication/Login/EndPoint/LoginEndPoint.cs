using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.Login.Command;
using ExaminationSystem.Features.Authentication.Login.Request;
using ExaminationSystem.Features.Authentication.Login.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.Login;

[ApiController]
[Route("api/auth")]
public class LoginEndPoint : ControllerBase
{
    private readonly IMediator _mediator;
    
    public LoginEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<RequestResult<LoginViewModel>>> Login ([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new LoginCommand(loginRequest.Email, loginRequest.Password), cancellationToken);
        
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.UserNotFound => Unauthorized(result), // 401
                ErrorCode.InvalidCredentials => Unauthorized(result),
                ErrorCode.Unauthorized => StatusCode(403, result),
                ErrorCode.TooManyRequests => StatusCode(429, result),
                _ => BadRequest(result)
            };
        }
        
        Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });
        
        var viewModel = new LoginViewModel()
        {
            AccessToken = result.Data.AccessToken,
        };
        
        return Ok(RequestResult<LoginViewModel>.Success(viewModel, "Success"));
    }
}