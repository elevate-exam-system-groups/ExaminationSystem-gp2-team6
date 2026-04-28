using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public record CheckAttemptLimitQuery(int QuizId, Guid StudentId) : IRequest<RequestResult<bool>>;
}
