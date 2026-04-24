using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using ExaminationSystem.Common.Pagination;
using ExaminationSystem.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries.HandlerCommands
{
    public sealed class GetStudentAttemptsQueryHandler
    : IRequestHandler<GetStudentAttemptsQuery, ExaminationSystem.Common.Pagination.PaginatedResult<AttemptSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStudentAttemptsQueryHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<ExaminationSystem.Common.Pagination.PaginatedResult<AttemptSummaryDto>> Handle(
            GetStudentAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.QuizAttempts.GetAll()
                .AsNoTracking()
                .Where(a => a.StudentId == request.StudentId)
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Diploma)
                .AsQueryable();

            if (request.QuizId.HasValue)
                query = query.Where(a => a.QuizId == request.QuizId.Value);

            if (request.DiplomaId.HasValue)
                query = query.Where(a => a.Quiz.DiplomaId == request.DiplomaId.Value);

            query = query.OrderByDescending(a => a.SubmittedAt ?? DateTime.MinValue);

            return await query
            .Select(a => new AttemptSummaryDto(
                a.Id,
                a.Quiz.Title,
                a.Score,
                a.Passed,
                a.Status.ToString(),
                a.SubmittedAt
            ))
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);
        }
    }
}
