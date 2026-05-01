using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends.UpdateDiploma
{
    public record UpdateDiplomaCommand(
    int DiplomaId,
    string Title,
    string? Description
) : IRequest<UpdateDiplomaResponse>;

    public record UpdateDiplomaResponse(
        int Id,
        string Title,
        string? Description,
        string Status
    );
}
