using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries.HandlerQueries
{
    public class IsQuizIdExistQueryHandler : IRequestHandler<IsQuizIdExistQuery, RequestResult<bool>>
    {
        private readonly AppDbContext _db;
        public IsQuizIdExistQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RequestResult<bool>> Handle(IsQuizIdExistQuery request, CancellationToken cancellationToken)
        {
            var exists = _db.Quizzes.Any(q => q.Id == request.QuizId);
            return exists
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found.");
        }
    }
}
