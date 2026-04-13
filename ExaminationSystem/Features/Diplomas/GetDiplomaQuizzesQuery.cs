using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Diplomas;

public sealed record GetDiplomaQuizzesQuery(int DiplomaId, Guid StudentId)
    : IRequest<RequestResult<IReadOnlyList<DiplomaQuizItemDto>>>;
