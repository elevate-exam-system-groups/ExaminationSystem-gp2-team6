using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator
{
    public record UpdateQuizOrchestrator(int Id, string? Title, int DiplomaId, int? DurationMinutes, int? PassScore, int? MaxAttempts, string? Instructions) : IRequest<RequestResult<bool>>;
}
