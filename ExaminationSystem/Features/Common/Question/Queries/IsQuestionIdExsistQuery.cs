using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Common.Question.Queries
{
    public record IsQuestionIdExsistQuery(int QuestionId) : IRequest<RequestResult<bool>>;
}
