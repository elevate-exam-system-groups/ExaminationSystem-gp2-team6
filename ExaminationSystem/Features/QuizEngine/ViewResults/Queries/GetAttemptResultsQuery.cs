using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries
{
    public sealed record GetAttemptResultsQuery(
    int AttemptId,
    int RequesterId,
    bool RequesterIsAdmin
) : IRequest<AttemptResultsDto>;
}
