using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands
{
    public record AutoSubmitAttemptCommand(int AttemptId) : IRequest<RequestResult<bool>>;
}

