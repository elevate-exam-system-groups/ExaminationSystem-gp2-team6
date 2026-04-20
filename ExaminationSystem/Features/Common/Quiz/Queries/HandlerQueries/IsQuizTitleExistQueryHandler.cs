using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Common.Quiz.Queries.HandlerQueries
{
    public class IsQuizTitleExistQueryHandler : IRequestHandler<IsQuizTitleExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IsQuizTitleExistQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<RequestResult<bool>> Handle(IsQuizTitleExistQuery request, CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.Quizzes.GetAll()
                .AsNoTracking()
                .AnyAsync(q => q.Title == request.Title, cancellationToken);
            return exists
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ExaminationSystem.Common.Data.ErrorCode.NotFound, "Quiz not found.");
        }
    }
}
