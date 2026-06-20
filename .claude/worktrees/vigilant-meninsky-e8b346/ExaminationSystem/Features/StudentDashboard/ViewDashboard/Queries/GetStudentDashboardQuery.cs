using ExaminationSystem.Common.Abstractions;
using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.ViewDashboard.Queries;

public sealed record GetStudentDashboardQuery(Guid StudentId)
    : ICacheableQuery<RequestResult<StudentDashboardResponseDto>>
{
    public string CacheKey => $"student_dashboard_{StudentId}";
    public TimeSpan? Expiration => TimeSpan.FromSeconds(60);
    public bool ShouldCacheResponse(RequestResult<StudentDashboardResponseDto> response) => response.IsSuccess;
}
