using ExaminationSystem.Common.Behaviors;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.Queries;

public sealed record GetStudentDashboardQuery(Guid StudentId)
    : ICacheableQuery<RequestResult<StudentDashboardResponseDto>>
{
    public string CacheKey => $"student_dashboard_{StudentId}";
    public TimeSpan? Expiration => TimeSpan.FromSeconds(60);
    public bool ShouldCacheResponse(RequestResult<StudentDashboardResponseDto> response) => response.IsSuccess;
}
