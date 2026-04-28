using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.ViewDashboard.Queries.HandlerQueries;

public class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, RequestResult<StudentDashboardResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentDashboardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResult<StudentDashboardResponseDto>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
        var enrolledDiplomas = await GetEnrolledDiplomasAsync(request.StudentId, cancellationToken);
        var attemptRows = await GetAttemptRowsAsync(request.StudentId, cancellationToken);

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

        return RequestResult<StudentDashboardResponseDto>.Success(response);
    }

    private Task<List<EnrolledDiplomaDto>> GetEnrolledDiplomasAsync(Guid studentId, CancellationToken cancellationToken)
    {
        return _unitOfWork.StudentDiplomaEnrollments.GetAll()
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .Select(e => new EnrolledDiplomaDto
            {
                Id = e.Diploma.Id,
                Title = e.Diploma.Title,
                Description = e.Diploma.Description,
                Status = e.Diploma.Status,
                TotalQuizCount = e.Diploma.Quizzes.Count
            })
            .ToListAsync(cancellationToken);
    }

    private Task<List<QuizAttemptRow>> GetAttemptRowsAsync(Guid studentId, CancellationToken cancellationToken)
    {
        return _unitOfWork.QuizAttempts.GetAll()
            .AsNoTracking()
            .Where(a => a.StudentId == studentId)
            .Select(a => new QuizAttemptRow
            {
                Id = a.Id,
                SubmittedAt = a.SubmittedAt,
                Score = a.Score,
                QuizTitle = a.Quiz.Title,
                PassScore = a.Quiz.PassScore
            })
            .ToListAsync(cancellationToken);
    }

    private sealed class QuizAttemptRow
    {
        public int Id { get; init; }
        public DateTime? SubmittedAt { get; init; }
        public double? Score { get; init; }
        public string QuizTitle { get; init; } = string.Empty;
        public int PassScore { get; init; }
    }
}
