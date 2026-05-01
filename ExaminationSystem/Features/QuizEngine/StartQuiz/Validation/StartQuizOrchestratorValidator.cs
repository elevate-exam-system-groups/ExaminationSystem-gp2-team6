using ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator;
using FluentValidation;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Validation;

public class StartQuizOrchestratorValidator : AbstractValidator<StartQuizOrchestrator>
{
    public StartQuizOrchestratorValidator()
    {
        RuleFor(x => x.QuizId)
            .GreaterThan(0).WithMessage("QuizId must be a positive integer.");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("StudentId is required.");
    }
}
