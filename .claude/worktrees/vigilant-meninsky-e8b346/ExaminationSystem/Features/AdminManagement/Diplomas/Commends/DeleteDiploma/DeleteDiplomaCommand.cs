using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.DeleteDiploma
{
    public record DeleteDiplomaCommand(int DiplomaId) : IRequest;
}
