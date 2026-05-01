using ExaminationSystem.Features.Authentication.ResetPassword.Command;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.ResetPassword.Validations;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(r => r.Token).NotEmpty().WithMessage("Token is required.");
        RuleFor(r => r.Password)
            .NotEmpty()
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain an uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain a lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain a number.");
        RuleFor(r => r.ConfirmedPassword)
            .Equal(r => r.Password).WithMessage("Passwords do not match.");
    }
}
