using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries.Handlers;

public sealed class GetDiplomaQuizzesQueryHandler
    : IRequestHandler<GetDiplomaQuizzesQuery, RequestResult<IReadOnlyList<DiplomaQuizItemDto>>>
{
    private readonly AppDbContext _db;

    public GetDiplomaQuizzesQueryHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RequestResult<IReadOnlyList<DiplomaQuizItemDto>>> Handle(
        GetDiplomaQuizzesQuery request,
        CancellationToken cancellationToken)
    {
        var diplomaExistsAndPublished = await _db.Diplomas
            .AsNoTracking()
            .AnyAsync(
                d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published,
                cancellationToken);

        if (!diplomaExistsAndPublished)
        {
            return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Failure(
                ErrorCode.NotFound,
                "Diploma not found.");
        }

        var isStudentEnrolled = await _db.StudentDiplomaEnrollments
            .AsNoTracking()
            .AnyAsync(
                e => e.DiplomaId == request.DiplomaId && e.StudentId == request.StudentId,
                cancellationToken);

        if (!isStudentEnrolled)
        {
            return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Failure(
                ErrorCode.Unauthorized,
                "Student is not enrolled in this diploma.");
        }

        var quizzes = await _db.Quizzes
            .AsNoTracking()
            .Where(q => q.DiplomaId == request.DiplomaId && q.Status == QuizStatus.Published)
            .OrderBy(q => q.Id)
            .Select(q => new DiplomaQuizItemDto
            {
                Id = q.Id,
                Title = q.Title,
                DurationMinutes = (int)q.Duration.TotalMinutes,
                AttemptCount = q.QuizAttempts.Count(a => a.StudentId == request.StudentId),
                LastScore = q.QuizAttempts
                    .Where(a => a.StudentId == request.StudentId)
                    .OrderByDescending(a => a.SubmittedAt)
                    .Select(a => (int?)a.Score)
                    .FirstOrDefault(),
                Status = q.Status.ToString().ToLowerInvariant()
            })
            .ToListAsync(cancellationToken);

        return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Success(quizzes);
    }
}
