using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries.Handlers;

public sealed class GetPublishedDiplomasQueryHandler
    : IRequestHandler<GetPublishedDiplomasQuery, RequestResult<GetPublishedDiplomasResponseDto>>
{
    private const int MaxPageSize = 100;
    private readonly IUnitOfWork _unitOfWork;

    public GetPublishedDiplomasQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResult<GetPublishedDiplomasResponseDto>> Handle(
        GetPublishedDiplomasQuery request,
        CancellationToken cancellationToken)
    {
        var page = request.PageNumber < 1 ? 1 : request.PageNumber;
        var perPage = request.PageSize < 1 ? 10 : Math.Min(request.PageSize, MaxPageSize);

        var baseQuery = _unitOfWork.Diplomas.GetAll()
            .AsNoTracking()
            .Where(d => d.Status == DiplomaStatus.Published);

        var total = await baseQuery.CountAsync(cancellationToken);

        var items = await baseQuery
            .OrderBy(d => d.Id)
            .Skip((page - 1) * perPage)
            .Take(perPage)
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
            .ToListAsync(cancellationToken);

        var response = new GetPublishedDiplomasResponseDto
        {
            Page = page,
            PerPage = perPage,
            Total = total,
            Items = items
        };

        return RequestResult<GetPublishedDiplomasResponseDto>.Success(response);
    }
}
