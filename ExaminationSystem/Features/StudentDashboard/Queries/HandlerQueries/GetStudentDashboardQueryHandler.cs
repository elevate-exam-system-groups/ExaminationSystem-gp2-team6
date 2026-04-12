using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard;
using ExaminationSystem.Features.StudentDashboard.Queries;
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
            .Select(a => new
            {
                a.Id,
                a.SubmittedAt,
                a.Score,
                QuizTitle = a.Quiz.Title,
                PassScore = a.Quiz.PassScore
            })
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

        OverallStatsDto overallStats;
        if (attemptRows.Count == 0)
        {
            overallStats = new OverallStatsDto();
        }
        else
        {
            var passed = attemptRows.Count(a => a.Score >= a.PassScore);
            overallStats = new OverallStatsDto
            {
                TotalQuizzesTaken = attemptRows.Count,
                AverageScore = Math.Round(attemptRows.Average(a => (double)a.Score), 2),
                PassRate = Math.Round(100.0 * passed / attemptRows.Count, 2)
            };
        }

        var response = new StudentDashboardResponseDto
        {
            EnrolledDiplomas = enrolledDiplomas,
            RecentQuizAttempts = recentAttempts,
            OverallStats = overallStats
        };

        _cache.Set(cacheKey, response, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheDuration });

        return RequestResult<StudentDashboardResponseDto>.Success(response);
    }
}
