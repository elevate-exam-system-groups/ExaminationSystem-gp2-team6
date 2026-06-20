using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Commands
{
    public record CreateQuizAttemptCommand(int QuizId, Guid StudentId) : IRequest<RequestResult<bool>>;
}
