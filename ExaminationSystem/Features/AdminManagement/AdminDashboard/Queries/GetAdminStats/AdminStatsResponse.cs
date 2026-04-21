namespace ExaminationSystem.Features.AdminManagement.AdminDashboard.Queries.GetAdminStats
{
    public record AdminStatsResponse(
        int TotalUsers,
        int ActiveUsersToday,
        int TotalQuizzes,
        int TotalAttempts,
        decimal AvgPassRate
    );
}
