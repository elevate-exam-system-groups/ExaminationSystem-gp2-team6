using ExaminationSystem.Features.Authentication.VerifyOtp.Request;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.VerifyOtp.Validations;

public class ResendOtpRequestValidator : AbstractValidator<ResendOtpRequest>
{
    public ResendOtpRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email");
    }
    
}