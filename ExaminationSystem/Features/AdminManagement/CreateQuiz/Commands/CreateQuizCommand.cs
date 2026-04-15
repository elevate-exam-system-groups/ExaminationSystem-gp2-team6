
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands
{
    public record CreateQuizCommand(string Title, TimeSpan Duration, int PassScore, int? MaxAttempts, QuizStatus Status, string? Instructions) : IRequest<RequestResult<CreateQuizCommand>>;
}
