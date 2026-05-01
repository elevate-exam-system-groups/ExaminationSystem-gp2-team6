using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.ForgetPassword.Command;
using ExaminationSystem.Features.Authentication.ForgetPassword.Request;
using ExaminationSystem.Features.Authentication.ForgetPassword.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.ForgetPassword.EndPoint;

[ApiController]
[Route("api/auth")]
public class ForgetPasswordEndPoint : ControllerBase
{
    private readonly IMediator _mediator;
    
    public ForgetPasswordEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("forget-password")]
    public async Task<ActionResult<RequestResult<ForgetPasswordViewModel>>> ForgetPassword ([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ForgetPasswordCommand(request.Email), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.UserNotFound => NotFound(result),
                ErrorCode.TooManyRequests => StatusCode(429, result),
                _ => BadRequest(result)
            };
        }
        
        var viewModel = new ForgetPasswordViewModel ()
        {
            UserId = result.Data.UserId,
            Email = result.Data.Email,
            Message = result.Data.Message
        };
        
        return Ok(RequestResult<ForgetPasswordViewModel>.Success(viewModel, "Success"));
    }
}