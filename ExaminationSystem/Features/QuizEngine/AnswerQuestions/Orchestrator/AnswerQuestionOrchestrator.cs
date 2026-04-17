using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Orchestrator
{
    public record AnswerQuestionOrchestrator(int AttemptId, int QuestionId, int? OptionId,Guid StudentId) : IRequest<RequestResult<bool>>;
}
