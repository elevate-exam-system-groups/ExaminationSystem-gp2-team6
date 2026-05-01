using FluentValidation;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.UpdateDiploma
{
    public class UpdateDiplomaCommandValidator : AbstractValidator<UpdateDiplomaCommand>
    {
        public UpdateDiplomaCommandValidator()
        {
            RuleFor(x => x.DiplomaId)
                .GreaterThan(0).WithMessage("Diploma ID must be valid.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
                .When(x => x.Description is not null);
        }
    }
}
