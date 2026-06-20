using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Authentication.SendOtp.Command;
using ExaminationSystem.Features.Authentication.VerifyOtp.Command;
using ExaminationSystem.Features.Authentication.VerifyOtp.Request;
using ExaminationSystem.Features.Authentication.VerifyOtp.VerifyOtpViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.Authentication.VerifyOtp.EndPoint;


[ApiController]
[Route("api/auth")]
public class VerifyOtpEndPoint : ControllerBase
{
    private readonly IMediator _mediator;

    public VerifyOtpEndPoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("verify-otp")]
    public async Task<ActionResult<RequestResult<VerifyOtpViewModel.VerifyOtpViewModel>>> VerifyOtp([FromBody] VerifyOtpRequest request , CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new VerifyOtpCommand(request.UserId, request.VerifyCode), cancellationToken);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.VerificationCodeExpired => BadRequest(result),
                ErrorCode.VerificationCodeInvalid => BadRequest(result),
                ErrorCode.UserNotFound => NotFound(result),
                _ => BadRequest(result)
            };
        }

        var viewModel = new VerifyOtpViewModel.VerifyOtpViewModel()
        {
            Token = result.Data.Token,
            ExpiresAt = result.Data.ExpiresAt,
            Message = result.Data.Message
        };
        
        return Ok(RequestResult<VerifyOtpViewModel.VerifyOtpViewModel>.Success(viewModel, "Success"));
    }
    
    [HttpPost("resend-otp")]
    public async Task<ActionResult<RequestResult<ResendOtpViewModel>>> ResendOtp([FromBody] ResendOtpRequest request , CancellationToken cancellationToken)
    {
        var resultSendOtp = await _mediator.Send(new SendOtpCommand(request.Email), cancellationToken);

        if (!resultSendOtp.IsSuccess)
        {
            return resultSendOtp.ErrorCode switch
            {
                ErrorCode.VerificationCodeExpired => BadRequest(resultSendOtp),
                ErrorCode.VerificationCodeInvalid => BadRequest(resultSendOtp),
                ErrorCode.UserNotFound => NotFound(resultSendOtp),
                _ => BadRequest(resultSendOtp)
            };
        }
        
        var tempraryToken = Guid.NewGuid().ToString();
        
        

        var viewModel = new ResendOtpViewModel()
        {
            Email = resultSendOtp.Data.Email,
            Token = resultSendOtp.Data.OtpCode,
            Message = resultSendOtp.Data.Message
        };
        
        return Ok(RequestResult<ResendOtpViewModel>.Success(viewModel, "Success"));
    }
}