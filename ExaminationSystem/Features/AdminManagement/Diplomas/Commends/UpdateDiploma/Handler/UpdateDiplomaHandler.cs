using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Diploma;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.UpdateDiploma.Handler
{
    public class UpdateDiplomaCommandHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<UpdateDiplomaCommand, UpdateDiplomaResponse>
    {
        public async Task<UpdateDiplomaResponse> Handle(
            UpdateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = await unitOfWork.Diplomas
                .GetByIdAsync(request.DiplomaId)
                ?? throw new NotFoundException(nameof(Diploma), request.DiplomaId);

            if (diploma.IsDeleted)
                throw new NotFoundException(nameof(Diploma), request.DiplomaId);

            unitOfWork.Diplomas.Update(diploma);

            diploma.Title = request.Title;
            diploma.Description = request.Description;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateDiplomaResponse(
                diploma.Id,
                diploma.Title,
                diploma.Description,
                diploma.Status.ToString());
        }
    }
}
