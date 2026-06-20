using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands
{
    public record AnswerQuestionCommand(int AttemptId, int QuestionId, int? OptionId) : IRequest<RequestResult<bool>>;
}
