using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Diplomas.Commends
{
    public record CreateDiplomaCommand(
     string Title,
     string? Description
     ) : IRequest<CreateDiplomaResponse>;
    public record CreateDiplomaResponse(
    int Id,
    string Title,
    string? Description,
    string Status
);
}
