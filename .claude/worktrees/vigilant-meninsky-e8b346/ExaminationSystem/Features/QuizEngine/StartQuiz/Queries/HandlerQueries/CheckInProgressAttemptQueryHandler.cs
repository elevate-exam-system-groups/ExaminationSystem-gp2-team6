using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Queries
{
    public class CheckInProgressAttemptQueryHandler : IRequestHandler<CheckInProgressAttemptQuery, RequestResult<bool>>
    {
		private readonly IUnitOfWork _uow;
		public CheckInProgressAttemptQueryHandler(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public async Task<RequestResult<bool>> Handle(CheckInProgressAttemptQuery request, CancellationToken cancellationToken)
        {
			var exists = await _uow.QuizAttempts.ExistsAsync(q => q.QuizId == request.QuizId && q.StudentId == request.StudentId && q.Status == AttemptStatus.InProgress);
			return exists
				? RequestResult<bool>.Success(true)
				: RequestResult<bool>.Failure(ExaminationSystem.Common.Data.ErrorCode.AttemptLimitReached, "You have reached the maximum number of attempts for this quiz.");
        }
    }
}