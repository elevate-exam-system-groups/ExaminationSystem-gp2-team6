using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;

namespace ExaminationSystem.Features.StudentDashboard;

public class StudentDashboardResponseDto
{
    public IReadOnlyList<EnrolledDiplomaDto> EnrolledDiplomas { get; init; } = Array.Empty<EnrolledDiplomaDto>();

    public IReadOnlyList<RecentQuizAttemptDto> RecentQuizAttempts { get; init; } = Array.Empty<RecentQuizAttemptDto>();

    public OverallStatsDto OverallStats { get; init; } = new();
}

public class EnrolledDiplomaDto
{
    public int Id { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public DiplomaStatus Status { get; init; }

    public int TotalQuizCount { get; init; }
}

public class RecentQuizAttemptDto
{
    public int AttemptId { get; init; }

    public string QuizTitle { get; init; } = null!;

    public double Score { get; init; }

    public bool Passed { get; init; }

    public DateTime SubmittedAt { get; init; }
}

public class OverallStatsDto
{
    public int TotalQuizzesTaken { get; init; }

    public double AverageScore { get; init; }

    public double PassRate { get; init; }
}
