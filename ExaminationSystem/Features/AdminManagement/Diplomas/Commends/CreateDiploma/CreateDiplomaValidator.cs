using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.CreateDiploma
{
    public class CreateDiplomaCommandValidator : AbstractValidator<CreateDiplomaCommand>
    {
        public CreateDiplomaCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
                .When(x => x.Description is not null);
        }
    }
}
