
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.AdminDashboard.Queries.GetAdminStats
{
    public record GetAdminStatsQuery : IRequest<AdminStatsResponse>;
}
