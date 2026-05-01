using ExaminationSystem.Features.Authentication.Register.Command;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.Register.Validations;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(r => r.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .Matches(@"^01[0-9]{9}$").WithMessage("Phone number must be a valid Egyptian number.");
        RuleFor(r => r.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("A valid email address is required.");
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
