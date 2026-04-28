using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;

public sealed record GetDiplomaQuizzesQuery(int DiplomaId, Guid StudentId)
    : IRequest<RequestResult<IReadOnlyList<DiplomaQuizItemDto>>>;
