using ExaminationSystem.Common.Views;
using ExaminationSystem.Common.Pagination;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries.Handlers;

public sealed class GetPublishedDiplomasQueryHandler
    : IRequestHandler<GetPublishedDiplomasQuery, RequestResult<PaginatedResult<PublishedDiplomaItemDto>>>
{
    private const int MaxPageSize = 100;
    private readonly IUnitOfWork _unitOfWork;

    public GetPublishedDiplomasQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResult<ExaminationSystem.Common.Pagination.PaginatedResult<PublishedDiplomaItemDto>>> Handle(
        GetPublishedDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Diplomas.GetAll()
            .AsNoTracking()
            .Where(d => d.Status == DiplomaStatus.Published);

        var result = await baseQuery
            .OrderBy(d => d.Id)
            .Select(d => new PublishedDiplomaItemDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                QuizCount = d.Quizzes.Count,
                StudentProgress = d.Quizzes.Count == 0
                    ? 0
                    : Math.Round(
                        100.0 *
                        d.Quizzes.Count(q =>
                            q.QuizAttempts.Any(a => a.StudentId == request.StudentId)) /
                        d.Quizzes.Count,
                        2)
            })
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return RequestResult<PaginatedResult<PublishedDiplomaItemDto>>.Success(result);
    }
}
