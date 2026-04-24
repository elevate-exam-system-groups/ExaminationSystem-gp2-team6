using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Validation
{
    public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Question text is required.");

            RuleFor(x => x.Options)
                .NotNull()
                .Must(x => x != null && x.Count >= 2).WithMessage("Options must have at least 2 items.")
                .Must(x => x != null && x.Count(o => o.IsCorrect) == 1).WithMessage("Exactly one correct option required.");
        }
    }
}
