using ExaminationSystem.Features.QuizEngine.StartQuiz.ViewModel;
using FluentValidation;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Validation
{
    public class StartQuizRequestViewModelValidator : AbstractValidator<StartQuizRequestViewModel>
    {
        public StartQuizRequestViewModelValidator() { 
            RuleFor(x => x.QuizId)
                .GreaterThan(0).WithMessage("QuizId must be greater than 0.");
            
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required.");
        }
    }
}
