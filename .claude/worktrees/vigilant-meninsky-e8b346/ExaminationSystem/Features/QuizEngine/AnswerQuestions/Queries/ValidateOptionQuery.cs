using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries
{
    public record ValidateOptionQuery(int QuestionId, int? OptionId) : IRequest<RequestResult<bool>>;
}
