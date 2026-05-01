using ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Validation;

public class UnpublishQuizCommandValidator : AbstractValidator<UnpublishQuizCommand>
{
    public UnpublishQuizCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .GreaterThan(0).WithMessage("QuizId must be a positive integer.");
    }
}
