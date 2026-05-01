using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries
{
    public record ValidateQuestionQuery(int QuizId, int QuestionId) : IRequest<RequestResult<bool>>;   
}
