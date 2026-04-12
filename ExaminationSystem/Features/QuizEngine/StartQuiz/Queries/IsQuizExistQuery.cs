using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.Common.Quizs.Queries
{
    public record IsQuizExistQuery():IRequest<RequestResult<bool>>;
}
