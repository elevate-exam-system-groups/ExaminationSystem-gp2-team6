using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator
{
    public record StartQuizOrchestrator(int QuizId, Guid StudentId) : IRequest<RequestResult<IEnumerable<QuestionDto>>>;    
}
