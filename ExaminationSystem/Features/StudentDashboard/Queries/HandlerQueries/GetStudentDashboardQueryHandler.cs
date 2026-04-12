using ExaminationSystem.Common.Views;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.StudentDashboard.Queries.HandlerQueries;

public class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, RequestResult<StudentDashboardResponseDto>>
{
    private const string CacheKeyPrefix = "student_dashboard_";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromSeconds(60);

    private readonly AppDbContext _db;
    private readonly IMemoryCache _cache;

    public GetStudentDashboardQueryHandler(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<RequestResult<StudentDashboardResponseDto>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeyPrefix + request.StudentId;
        if (_cache.TryGetValue(cacheKey, out StudentDashboardResponseDto? cached) && cached is not null)
            return RequestResult<StudentDashboardResponseDto>.Success(cached);

        var enrolledDiplomas = await _db.StudentDiplomaEnrollments
            .AsNoTracking()
            .Where(e => e.StudentId == request.StudentId)
            .Select(e => new EnrolledDiplomaDto
            {
                Id = e.Diploma.Id,
                Title = e.Diploma.Title,
                Description = e.Diploma.Description,
                Status = e.Diploma.Status,
                TotalQuizCount = e.Diploma.Quizzes.Count
            })
            .ToListAsync(cancellationToken);

        var attemptRows = await _db.QuizAttempts
            .AsNoTracking()
            .Where(a => a.StudentId == request.StudentId)
            .Select(a => new AttemptRow(
                a.Id,
                a.SubmittedAt,
                a.Score,
                a.Quiz.Title,
                a.Quiz.PassScore))
            .ToListAsync(cancellationToken);

        var recentAttempts = attemptRows
            .OrderByDescending(a => a.SubmittedAt)
            .Take(20)
            .Select(a => new RecentQuizAttemptDto
            {
                AttemptId = a.Id,
                QuizTitle = a.QuizTitle,
                Score = a.Score,
                Passed = a.Score >= a.PassScore,
                SubmittedAt = a.SubmittedAt
            })
            .ToList();

        var overallStats = BuildOverallStats(attemptRows);

        var response = new StudentDashboardResponseDto
        {
            EnrolledDiplomas = enrolledDiplomas,
            RecentQuizAttempts = recentAttempts,
            OverallStats = overallStats
        };

        _cache.Set(cacheKey, response, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration });

        return RequestResult<StudentDashboardResponseDto>.Success(response);
    }

    private static OverallStatsDto BuildOverallStats(List<AttemptRow> rows)
    {
        if (rows.Count == 0)
            return new OverallStatsDto();

        var passed = rows.Count(r => r.Score >= r.PassScore);
        return new OverallStatsDto
        {
            TotalQuizzesTaken = rows.Count,
            AverageScore = Math.Round(rows.Average(r => (double)r.Score), 2),
            PassRate = Math.Round(100.0 * passed / rows.Count, 2)
        };
    }

    private readonly record struct AttemptRow(int Id, DateTime SubmittedAt, int Score, string QuizTitle, int PassScore);
}
