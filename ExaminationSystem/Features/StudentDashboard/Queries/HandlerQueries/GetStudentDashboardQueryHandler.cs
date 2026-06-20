using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard;
using ExaminationSystem.Features.StudentDashboard.Queries;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.Queries.HandlerQueries;

public class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, RequestResult<StudentDashboardResponseDto>>
{
    private readonly AppDbContext _db;

    public GetStudentDashboardQueryHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RequestResult<StudentDashboardResponseDto>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
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
                Score = a.Score??0,
                Passed = a.PassScore != 0 ? a.Score / a.PassScore >= 1.0 : false,
                SubmittedAt = a.SubmittedAt?? DateTime.MinValue
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

        return RequestResult<StudentDashboardResponseDto>.Success(response);
    }
}
