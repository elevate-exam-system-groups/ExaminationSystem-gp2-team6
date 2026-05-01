using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.ResetPassword.Command;
using ExaminationSystem.Features.Authentication.ResetPassword.Request;
using ExaminationSystem.Features.Authentication.ResetPassword.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.ResetPassword.EndPoint;

[ApiController]
[Route("api/auth")]
public class ResetPasswordEndPoint : ControllerBase
{
    private readonly IMediator _mediator;

    public ResetPasswordEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("reset-password")]
    public async Task<ActionResult<RequestResult<ResetPasswordViewModel>>> ResetPassword(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ResetPasswordCommand(request.UserId, request.Token, request.Password, request.ConfirmedPassword), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.PasswordNotMatch => BadRequest(result),
                ErrorCode.InvalidCredentials => BadRequest(result),
                ErrorCode.UserNotFound => NotFound(result),
                _ => BadRequest(result)
            };
        }
        
        var viewModel = new ResetPasswordViewModel()
        {
            Message = "Password reset successful",
        };
        
        return Ok(RequestResult<ResetPasswordViewModel>.Success(viewModel, "Success"));
    }
    
}