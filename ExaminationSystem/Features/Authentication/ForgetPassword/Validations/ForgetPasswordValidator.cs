using ExaminationSystem.Features.Authentication.ForgetPassword.Request;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.ForgetPassword.Validations;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest>
{
    ForgetPasswordValidator()
    {
        RuleFor(f => f.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();
    }
}