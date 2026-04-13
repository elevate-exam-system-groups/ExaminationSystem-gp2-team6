using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Diplomas.Queries;

public sealed record GetDiplomaQuizzesQuery(int DiplomaId, Guid StudentId)
    : IRequest<RequestResult<IReadOnlyList<DiplomaQuizItemDto>>>;
