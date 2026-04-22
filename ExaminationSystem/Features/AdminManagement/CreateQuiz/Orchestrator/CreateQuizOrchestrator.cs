using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Orchestrator
{
    public record CreateQuizOrchestrator(string Title, int DiplomaId, int DurationMinutes, int PassScore, int? MaxAttempts, string? Instructions) : IRequest<RequestResult<bool>>;
}
