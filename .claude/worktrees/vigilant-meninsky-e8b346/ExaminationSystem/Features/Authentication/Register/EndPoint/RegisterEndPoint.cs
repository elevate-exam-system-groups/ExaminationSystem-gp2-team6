using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.Register.Command;
using ExaminationSystem.Features.Authentication.Register.Request;
using ExaminationSystem.Features.Authentication.Register.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.Register.EndPoint;

[ApiController]
[Route("api/auth")]
public class RegisterEndPoint : ControllerBase
{
    private readonly IMediator _mediator;
    
    public RegisterEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<RequestResult<RegisterViewModel>>> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RegisterCommand(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email, registerRequest.PhoneNumber, registerRequest.Password, registerRequest.ConfirmedPassword), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.UserAlreadyExists => Conflict(result),
                ErrorCode.ValidationError => BadRequest(result),
                ErrorCode.InvalidData => BadRequest(result),
                _ => StatusCode(500, result)
            };
        }
        
        Response.Cookies.Append("refreshToken", result.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        var viewModel = new RegisterViewModel()
        {
            Email = registerRequest.Email,
            AccessToken = result.Data.AccessToken,
        };
        
        return Ok(RequestResult<RegisterViewModel>.Success(viewModel, "Success"));
    }
}