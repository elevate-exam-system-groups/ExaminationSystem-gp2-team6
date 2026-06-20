using ExaminationSystem.Features.Authentication.Register.Request;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.Register.Validations;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Matches(@"^01[0-9]{9}$")
            .WithMessage("Invalid phone number");

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must have at least 8 characters")
            .Matches("[A-Z]").WithMessage("Must contain uppercase letter")
            .Matches("[a-z]").WithMessage("Must contain lowercase letter")
            .Matches("[0-9]").WithMessage("Must contain a number");

        RuleFor(r => r.ConfirmedPassword)
            .NotEmpty().WithMessage("Confirmed password is required")
            .Equal(r => r.Password).WithMessage("Passwords do not match");
    }
}