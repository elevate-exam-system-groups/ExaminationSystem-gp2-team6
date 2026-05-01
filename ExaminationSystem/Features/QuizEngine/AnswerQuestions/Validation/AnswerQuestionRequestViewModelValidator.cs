using ExaminationSystem.Features.QuizEngine.AnswerQuestions.ViewModels;
using FluentValidation;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Validation
{
    public class AnswerQuestionRequestViewModelValidator: AbstractValidator<AnswerQuestionRequestViewModel>
    {
        public AnswerQuestionRequestViewModelValidator()
        {
            RuleFor(x => x.AttemptId).GreaterThan(0).WithMessage("AttemptId must be greater than 0.");
            RuleFor(x => x.QuestionId).GreaterThan(0).WithMessage("QuestionId must be greater than 0.");
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("StudentId is required.");
            RuleFor(x => x.OptionId).GreaterThan(0).WithMessage("OptionId must be greater than 0.");
        }
    }
}
