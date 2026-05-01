using ExaminationSystem.Features.AdminManagement.CreateQuiz.ViewModel;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Validation
{
    public class CreateQuizRequestViewModelValidator : AbstractValidator<CreateQuizRequestViewModel>
    {
        public CreateQuizRequestViewModelValidator()
        {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is required.")
                    .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
    
                RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be a positive integer.");
    
                RuleFor(x => x.PassScore)
                    .InclusiveBetween(0, 100).WithMessage("Pass score must be between 0 and 100.");
    
                RuleFor(x => x.MaxAttempts)
                    .GreaterThan(0).When(x => x.MaxAttempts.HasValue).WithMessage("Max attempts must be greater than zero when specified.");
    
                RuleFor(x => x.Instructions)
                    .MaximumLength(500).WithMessage("Instructions must not exceed 500 characters.");
        }
    }
}
