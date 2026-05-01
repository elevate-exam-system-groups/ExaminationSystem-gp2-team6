using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands.HandlerCommands
{
    public class UnpublishQuizCommandHandler : IRequestHandler<UnpublishQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public UnpublishQuizCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _uow.Quizzes.GetAll(withNoTracking: false)
                .FirstOrDefaultAsync(q => q.Id == request.QuizId && !q.IsDeleted, cancellationToken);

            if (quiz == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found.");

            if (quiz.Status == QuizStatus.Draft)
                return RequestResult<bool>.Success(true);

            quiz.Status = QuizStatus.Draft;

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.Unknown, "Failed to unpublish quiz.");
        }
    }
}
