using FluentValidation;
using ResetPasswordRequest = ExaminationSystem.Features.Authentication.ResetPassword.Request.ResetPasswordRequest;

namespace ExaminationSystem.Features.Authentication.ResetPassword.Validations;

public class ResetPasswordRequestValiditor : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValiditor()
    {
        RuleFor(r => r.UserId)
            .NotNull().WithMessage("UserId cannot be null");
        
        RuleFor(r => r.Token)
            .NotNull().WithMessage("Token cannot be null");
        
        RuleFor(r => r.Password)
            .NotNull().WithMessage("Password cannot be null")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters")
            .Matches("[A-Z]").WithMessage("Must contain uppercase letter")
            .Matches("[a-z]").WithMessage("Must contain lowercase letter")
            .Matches("[0-9]").WithMessage("Must contain a number");

        RuleFor(r => r.ConfirmedPassword)
            .NotEmpty().WithMessage("Confirmed password is required")
            .Equal(r => r.Password).WithMessage("Passwords do not match");
    }
}