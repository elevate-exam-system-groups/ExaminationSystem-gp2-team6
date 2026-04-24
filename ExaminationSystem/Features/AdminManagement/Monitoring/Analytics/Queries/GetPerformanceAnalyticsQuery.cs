using ExaminationSystem.Common.Abstractions;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Dtos;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Queries
{
    public sealed record GetPerformanceAnalyticsQuery(
        DateTime? From,
        DateTime? To,
        int? DiplomaId
    ) : ICacheableQuery<RequestResult<AnalyticsDashboardDto>>
    {
        public string CacheKey => $"Analytics_{From?.ToString("o")}_{To?.ToString("o")}_{DiplomaId}";

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);

        public bool ShouldCacheResponse(RequestResult<AnalyticsDashboardDto> response) => response.IsSuccess;
    }
}
