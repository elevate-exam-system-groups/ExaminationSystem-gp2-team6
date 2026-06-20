using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands
{
    public record CreateQuizCommand(string Title, int DiplomaId, int DurationMinutes, int PassScore, int? MaxAttempts, string? Instructions) : IRequest<RequestResult<bool>>;
}
