using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public record IsQuizIdExistQuery(int QuizId) : IRequest<RequestResult<bool>>;
}
