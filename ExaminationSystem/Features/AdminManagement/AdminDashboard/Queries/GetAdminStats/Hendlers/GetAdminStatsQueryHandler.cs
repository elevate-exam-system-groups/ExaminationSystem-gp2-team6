using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Features.AdminManagement.AdminDashboard.Queries.GetAdminStats.Hendlers
{
    public class GetAdminStatsQueryHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IMemoryCache cache) : IRequestHandler<GetAdminStatsQuery, AdminStatsResponse>
    {
        private const string CacheKey = "admin_stats";

        public async Task<AdminStatsResponse> Handle(
            GetAdminStatsQuery request, CancellationToken cancellationToken)
        {
            if (cache.TryGetValue(CacheKey, out AdminStatsResponse? cached))
                return cached!;

            int totalUsers = await userRepository.CountAsync();

            var todayUtc = DateTime.UtcNow.Date;

            int activeUsersToday = await unitOfWork.LoginLogs
                .GetAll()
                .Where(l => l.LoggedInAt >= todayUtc)
                .Select(l => l.UserId)
                .Distinct()
                .CountAsync(cancellationToken);

            int totalQuizzes = await unitOfWork.Quizzes
                .GetAll()
                .Where(q => !q.IsDeleted)
                .CountAsync(cancellationToken);

            int totalAttempts = await unitOfWork.QuizAttempts
                .GetAll()
                .Where(a => !a.IsDeleted)
                .CountAsync(cancellationToken);

            var completedAttempts = unitOfWork.QuizAttempts
                .GetAll()
                .Where(a => !a.IsDeleted
                    && (a.Status == AttemptStatus.Submitted
                        || a.Status == AttemptStatus.TimedOut));

            int completedCount = await completedAttempts.CountAsync(cancellationToken);

            decimal avgPassRate = 0;
            if (completedCount > 0)
            {
                int passedCount = await completedAttempts
                    .CountAsync(a => a.Passed == true, cancellationToken);

                avgPassRate = Math.Round((decimal)passedCount / completedCount * 100, 2);
            }

            var response = new AdminStatsResponse(
                totalUsers,
                activeUsersToday,
                totalQuizzes,
                totalAttempts,
                avgPassRate);

            cache.Set(CacheKey, response, TimeSpan.FromMinutes(5));

            return response;
        }
    }
}
