using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using ExaminationSystem.Common.Pagination;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries
{
    public sealed record GetStudentAttemptsQuery(
     Guid StudentId,
     int? QuizId,
     int? DiplomaId,
     int Page = 1,
     int PageSize = 10
 ) : IRequest<PaginatedResult<AttemptSummaryDto>>;
}
