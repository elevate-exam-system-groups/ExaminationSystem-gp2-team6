using ExaminationSystem.Features.Authentication.VerifyOtp.Request;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.VerifyOtp.Validations;

public class VerifyOtpValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpValidator()
    {
        RuleFor(r => r.UserId);
        RuleFor(r => r.VerifyCode)
            .NotEmpty().WithMessage("VerifyCode is required");
    }
}