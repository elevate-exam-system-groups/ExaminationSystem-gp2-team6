using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries
{
    public sealed record GetStudentAttemptsQuery(
     int StudentId,
     int? QuizId,
     int? DiplomaId,
     int Page = 1,
     int PerPage = 10
 ) : IRequest<PagedResult<AttemptSummaryDto>>;
}
