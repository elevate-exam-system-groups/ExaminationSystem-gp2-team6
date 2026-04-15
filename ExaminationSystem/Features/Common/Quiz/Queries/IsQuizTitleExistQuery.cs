using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Common.Quiz.Queries
{
    public record IsQuizTitleExistQuery(string Title):IRequest<RequestResult<bool>>;
}
