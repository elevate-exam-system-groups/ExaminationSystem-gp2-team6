using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.Common.Quizs.Queries;
using MediatR;

namespace ExaminationSystem.Features.Quiz_Engine.StartQuiz.Queries.HandlerQueries
{
    public class IsQuizExistQueryHandler : IRequestHandler<IsQuizExistQuery, RequestResult<bool>>
    {
        public async Task<RequestResult<bool>> Handle(IsQuizExistQuery request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }
}
