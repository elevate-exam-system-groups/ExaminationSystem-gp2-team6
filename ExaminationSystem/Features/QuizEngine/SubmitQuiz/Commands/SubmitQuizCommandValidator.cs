using FluentValidation;

namespace ExaminationSystem.Features.QuizEngine.SubmitQuiz.Commands
{
    public sealed class SubmitQuizCommandValidator : AbstractValidator<SubmitQuizCommand>
    {
        public SubmitQuizCommandValidator()
        {
            RuleFor(x => x.AttemptId)
                .GreaterThan(0).WithMessage("AttemptId must be a valid positive integer.");

            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required.");
        }
    }
}
