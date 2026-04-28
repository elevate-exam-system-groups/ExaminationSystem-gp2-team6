using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.CreateQuiz;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public record BuildStartQuizResponseQuery(int QuizId, Guid StudentId):IRequest<RequestResult<IEnumerable<QuestionDto>>>;
}
