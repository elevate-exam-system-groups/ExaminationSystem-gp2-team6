using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;

public sealed record GetPublishedDiplomasQuery(Guid StudentId, int PageNumber = 1, int PageSize = 10)
    : IRequest<RequestResult<GetPublishedDiplomasResponseDto>>;
