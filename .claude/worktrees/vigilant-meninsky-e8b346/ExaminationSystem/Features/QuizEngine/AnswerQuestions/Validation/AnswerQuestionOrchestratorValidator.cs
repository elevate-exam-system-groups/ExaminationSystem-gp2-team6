using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Orchestrator;
using FluentValidation;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Validation;

public class AnswerQuestionOrchestratorValidator : AbstractValidator<AnswerQuestionOrchestrator>
{
    public AnswerQuestionOrchestratorValidator()
    {
        RuleFor(x => x.AttemptId)
            .GreaterThan(0).WithMessage("AttemptId must be a positive integer.");

        RuleFor(x => x.QuestionId)
            .GreaterThan(0).WithMessage("QuestionId must be a positive integer.");

        RuleFor(x => x.StudentId)
            .NotEmpty().WithMessage("StudentId is required.");

        RuleFor(x => x.OptionId)
            .GreaterThan(0).When(x => x.OptionId.HasValue)
            .WithMessage("OptionId must be a positive integer if specified.");
    }
}
