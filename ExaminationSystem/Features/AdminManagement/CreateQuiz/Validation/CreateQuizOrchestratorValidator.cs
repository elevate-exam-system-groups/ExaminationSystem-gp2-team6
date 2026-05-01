using ExaminationSystem.Features.AdminManagement.CreateQuiz.Orchestrator;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Validation;

public class CreateQuizOrchestratorValidator : AbstractValidator<CreateQuizOrchestrator>
{
    public CreateQuizOrchestratorValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.DiplomaId)
            .GreaterThan(0).WithMessage("DiplomaId must be a positive integer.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be a positive number of minutes.");

        RuleFor(x => x.PassScore)
            .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");

        RuleFor(x => x.MaxAttempts)
            .GreaterThan(0).When(x => x.MaxAttempts.HasValue)
            .WithMessage("Max attempts must be positive if specified.");

        RuleFor(x => x.Instructions)
            .MaximumLength(500).When(x => x.Instructions != null)
            .WithMessage("Instructions cannot exceed 500 characters.");
    }
}
