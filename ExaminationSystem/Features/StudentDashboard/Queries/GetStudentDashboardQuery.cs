using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.Queries;

public record GetStudentDashboardQuery(Guid StudentId) : IRequest<RequestResult<StudentDashboardResponseDto>>;
