using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.DeleteDiploma.Handler
{
    public class DeleteDiplomaCommandHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<DeleteDiplomaCommand>
    {
        public async Task Handle(
            DeleteDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await unitOfWork.Diplomas.GetAll(withNoTracking: false)
                .FirstOrDefaultAsync(d => d.Id == request.DiplomaId && !d.IsDeleted, cancellationToken)
                ?? throw new NotFoundException(nameof(Diploma), request.DiplomaId);

            if (diploma.Status == DiplomaStatus.Published)
            {
                var hasEnrollments = await unitOfWork.StudentDiplomaEnrollments
                    .ExistsAsync(e => e.DiplomaId == request.DiplomaId);

                if (hasEnrollments)
                    throw new ConflictException(
                        "Cannot delete a published diploma with active student enrollments.");
            }

            unitOfWork.Diplomas.SoftDelete(diploma);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
