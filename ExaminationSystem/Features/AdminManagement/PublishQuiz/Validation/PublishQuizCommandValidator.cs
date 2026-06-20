using ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz.Validation;

public class PublishQuizCommandValidator : AbstractValidator<PublishQuizCommand>
{
    public PublishQuizCommandValidator()
    {
        RuleFor(x => x.QuizId)
            .GreaterThan(0).WithMessage("QuizId must be a positive integer.");
    }
}
