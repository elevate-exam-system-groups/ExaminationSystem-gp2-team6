using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries.HandlerQueries
{
    public class IsQuizIdExistQueryHandler : IRequestHandler<IsQuizIdExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public IsQuizIdExistQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(IsQuizIdExistQuery request, CancellationToken cancellationToken)
        {
            var exists = await _uow.Quizzes.ExistsAsync(q => q.Id == request.QuizId);
            return exists
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found.");
        }
    }
}
