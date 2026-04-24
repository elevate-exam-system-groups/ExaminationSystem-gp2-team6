using ExaminationSystem.Common.Views;
using ExaminationSystem.Common.Pagination;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries
{
    public record GetAttemptsQuery(
        Guid? StudentId,
        int? QuizId,
        int Page = 1,
        int PageSize = 20,
        string SortBy = "submitted_at",
        string Order = "desc"
    ) : IRequest<RequestResult<PaginatedResult<AttemptSummaryDto>>>;
}
