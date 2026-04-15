using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public class CheckInProgressAttemptQueryHandler : IRequestHandler<CheckInProgressAttemptQuery, RequestResult<bool>>
    {
		private readonly AppDbContext _db;
		public CheckInProgressAttemptQueryHandler(AppDbContext db)
		{
			_db = db;
		}

		public async Task<RequestResult<bool>> Handle(CheckInProgressAttemptQuery request, CancellationToken cancellationToken)
        {
			var exists = _db.QuizAttempts.Any(q => q.QuizId == request.QuizId && q.StudentId == request.StudentId && q.Status == AttemptStatus.InProgress);
			return exists
				? RequestResult<bool>.Success(true)
				: RequestResult<bool>.Failure(ExaminationSystem.Common.Data.ErrorCode.AttemptLimitReached, "You have reached the maximum number of attempts for this quiz.");
        }
    }
}