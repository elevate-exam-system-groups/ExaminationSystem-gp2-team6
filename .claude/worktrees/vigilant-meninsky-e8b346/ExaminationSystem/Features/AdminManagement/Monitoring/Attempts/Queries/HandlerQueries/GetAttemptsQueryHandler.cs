using ExaminationSystem.Common.Views;
using ExaminationSystem.Common.Pagination;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries.HandlerQueries
{
    public class GetAttemptsQueryHandler : IRequestHandler<GetAttemptsQuery, RequestResult<PaginatedResult<AttemptSummaryDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetAttemptsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<PaginatedResult<AttemptSummaryDto>>> Handle(GetAttemptsQuery request, CancellationToken cancellationToken)
        {
            var query = _uow.QuizAttempts.GetAll();

            if (request.StudentId.HasValue)
            {
                query = query.Where(a => a.StudentId == request.StudentId.Value);
            }

            if (request.QuizId.HasValue)
            {
                query = query.Where(a => a.QuizId == request.QuizId.Value);
            }

            if (request.SortBy.ToLower() == "submitted_at")
            {
                query = request.Order.ToLower() == "asc" 
                    ? query.OrderBy(a => a.SubmittedAt) 
                    : query.OrderByDescending(a => a.SubmittedAt);
            }
            else
            {
                query = query.OrderByDescending(a => a.SubmittedAt);
            }

            var result = await query
                .Select(a => new AttemptSummaryDto
                {
                    AttemptId = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.FullName,
                    QuizTitle = a.Quiz.Title,
                    Score = (int)(a.Score ?? 0),
                    Status = a.Status,
                    SubmittedAt = a.SubmittedAt ?? DateTime.MinValue
                })
                .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

            return RequestResult<PaginatedResult<AttemptSummaryDto>>.Success(result);
        }
    }
}
