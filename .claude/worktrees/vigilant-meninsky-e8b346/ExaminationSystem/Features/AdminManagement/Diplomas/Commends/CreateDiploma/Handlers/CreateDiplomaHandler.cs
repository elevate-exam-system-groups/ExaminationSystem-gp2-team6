using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.CreateDiploma.Handlers
{
    public class CreateDiplomaCommandHandler(IUnitOfWork unitOfWork)
     : IRequestHandler<CreateDiplomaCommand, CreateDiplomaResponse>
    {
        public async Task<CreateDiplomaResponse> Handle(
            CreateDiplomaCommand request, CancellationToken cancellationToken)
        {
            var diploma = new Diploma
            {
                Title = request.Title,
                Description = request.Description,
                Status = DiplomaStatus.Draft
            };

            await unitOfWork.Diplomas.AddAsync(diploma);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateDiplomaResponse(
                diploma.Id,
                diploma.Title,
                diploma.Description,
                diploma.Status.ToString());
        }
    }
}
