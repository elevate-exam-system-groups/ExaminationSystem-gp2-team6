using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.StudentDashboard;
using MediatR;

namespace ExaminationSystem.Features.StudentDashboard.Queries;

public record GetStudentDashboardQuery(Guid StudentId) : IRequest<RequestResult<StudentDashboardResponseDto>>;
