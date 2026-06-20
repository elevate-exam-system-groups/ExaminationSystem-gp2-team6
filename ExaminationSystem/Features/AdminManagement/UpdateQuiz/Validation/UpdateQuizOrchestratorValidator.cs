using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Validation;

public class UpdateQuizOrchestratorValidator : AbstractValidator<UpdateQuizOrchestrator>
{
    public UpdateQuizOrchestratorValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Quiz ID must be a positive integer.");

        RuleFor(x => x.Title)
            .MaximumLength(100).When(x => x.Title != null)
            .WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.DiplomaId)
            .GreaterThan(0).WithMessage("DiplomaId must be a positive integer.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).When(x => x.DurationMinutes.HasValue)
            .WithMessage("Duration must be a positive number of minutes.");

        RuleFor(x => x.PassScore)
            .InclusiveBetween(0, 100).When(x => x.PassScore.HasValue)
            .WithMessage("Pass score must be between 0 and 100.");

        RuleFor(x => x.MaxAttempts)
            .GreaterThan(0).When(x => x.MaxAttempts.HasValue)
            .WithMessage("Max attempts must be positive if specified.");

        RuleFor(x => x.Instructions)
            .MaximumLength(500).When(x => x.Instructions != null)
            .WithMessage("Instructions cannot exceed 500 characters.");
    }
}
