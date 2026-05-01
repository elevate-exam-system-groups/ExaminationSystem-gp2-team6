using ExaminationSystem.Features.Authentication.Token.Command;
using FluentValidation;

namespace ExaminationSystem.Features.Authentication.Token.Validations;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(r => r.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required.");
    }
}
