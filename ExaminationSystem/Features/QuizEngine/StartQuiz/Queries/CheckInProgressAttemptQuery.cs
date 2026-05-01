using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public record CheckInProgressAttemptQuery(int QuizId, Guid StudentId) : IRequest<RequestResult<bool>>;
}
