using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries.HandlerCommands
{
    public sealed class GetStudentAttemptsQueryHandler
    : IRequestHandler<GetStudentAttemptsQuery, PagedResult<AttemptSummaryDto>>
    {
        private readonly AppDbContext _context;

        public GetStudentAttemptsQueryHandler(AppDbContext context)
            => _context = context;

        public async Task<PagedResult<AttemptSummaryDto>> Handle(
            GetStudentAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Attempts
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

            int totalCount = await query.CountAsync(cancellationToken);

            int page = request.Page < 1 ? 1 : request.Page;
            int perPage = request.PerPage < 1 ? 10 : request.PerPage;

            var items = await query
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .Select(a => new
            {
                a.Id,
                QuizTitle = a.Quiz.Title,
                a.Score,
                a.Passed,
                a.Status,
                a.SubmittedAt
            })
            .ToListAsync(cancellationToken);

            var dtos = items.Select(a => new AttemptSummaryDto(
                AttemptId: a.Id,
                QuizTitle: a.QuizTitle,
                Score: a.Score,
                Passed: a.Passed,
                Status: a.Status.ToString(),
                SubmittedAt: a.SubmittedAt
            )).ToList();

            int totalPages = totalCount == 0
                ? 1
                : (int)Math.Ceiling((double)totalCount / perPage);

            return new PagedResult<AttemptSummaryDto>(dtos, totalCount, page, perPage, totalPages);
        }
    }
}
