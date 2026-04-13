using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Diplomas;

public sealed record GetPublishedDiplomasQuery(Guid StudentId, int PageNumber = 1, int PageSize = 10)
    : IRequest<RequestResult<GetPublishedDiplomasResponseDto>>;
