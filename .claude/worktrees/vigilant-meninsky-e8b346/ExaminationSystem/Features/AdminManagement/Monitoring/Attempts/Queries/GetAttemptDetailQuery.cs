using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries
{
    public record GetAttemptDetailQuery(int AttemptId) : IRequest<RequestResult<AttemptDetailDto>>;
}
