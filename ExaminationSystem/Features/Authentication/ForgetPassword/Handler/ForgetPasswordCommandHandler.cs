using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.User.ApplicationUser;
using ExaminationSystem.Features.Authentication.ForgetPassword.Command;
using ExaminationSystem.Features.Authentication.ForgetPassword.Dto;
using ExaminationSystem.Features.Authentication.SendOtp.Command;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Features.Authentication.ForgetPassword.Handler;

public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, RequestResult<ForgetPasswordDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMediator _mediator;

    public ForgetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator)
    {
        _userManager = userManager;
        _mediator = mediator;
    }
    
    public async Task<RequestResult<ForgetPasswordDto>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null) return RequestResult<ForgetPasswordDto>.Failure(ErrorCode.UserNotFound, "User not found");
        
        var otpResult = await _mediator.Send(new SendOtpCommand(request.Email), cancellationToken);
        
        if (!otpResult.IsSuccess) return RequestResult<ForgetPasswordDto>.Failure(otpResult.ErrorCode, otpResult.Message);

        var dto = new ForgetPasswordDto
        {
            UserId = user.Id,
            Email = otpResult.Data.Email,
            Message = "OTP sent successfully",
            OtpCode = otpResult.Data.OtpCode
        };
        
        return RequestResult<ForgetPasswordDto>.Success(dto, "Success");
    }
}