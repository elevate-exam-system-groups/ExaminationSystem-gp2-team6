using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{    
    public record GetQuestionsByQuizIdQuery(int QuizId) : IRequest<RequestResult<IEnumerable<QuestionDto>>>;
}
