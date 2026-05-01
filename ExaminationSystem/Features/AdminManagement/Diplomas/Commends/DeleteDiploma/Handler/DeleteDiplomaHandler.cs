using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.DeleteDiploma.Handler
{
    public class DeleteDiplomaCommandHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<DeleteDiplomaCommand>
    {
        public async Task Handle(
            DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await unitOfWork.Diplomas
                .GetByIdAsync(request.DiplomaId, d => d.StudentEnrollments)
                ?? throw new NotFoundException(nameof(Diploma), request.DiplomaId);

            if (diploma.IsDeleted)
                throw new NotFoundException(nameof(Diploma), request.DiplomaId);

            if (diploma.Status == DiplomaStatus.Published
                && diploma.StudentEnrollments.Any())
            {
                throw new ConflictException(
                    "Cannot delete a published diploma with active student enrollments.");
            }

            unitOfWork.Diplomas.SoftDelete(diploma);

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}