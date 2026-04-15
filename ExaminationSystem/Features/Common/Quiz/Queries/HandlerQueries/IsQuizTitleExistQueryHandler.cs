using ExaminationSystem.Common.Views;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;

namespace ExaminationSystem.Features.Common.Quiz.Queries.HandlerQueries
{
    public class IsQuizTitleExistQueryHandler : IRequestHandler<IsQuizTitleExistQuery, RequestResult<bool>>
    {
        private readonly AppDbContext _db;
        public IsQuizTitleExistQueryHandler(AppDbContext db)
        {
            _db = db;
        }
        public async Task<RequestResult<bool>> Handle(IsQuizTitleExistQuery request, CancellationToken cancellationToken)
        {
            var exists = _db.Quizzes.Any(q => q.Title == request.Title);
            return exists
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ExaminationSystem.Common.Data.ErrorCode.NotFound, "Quiz not found.");
        }
    }
}
