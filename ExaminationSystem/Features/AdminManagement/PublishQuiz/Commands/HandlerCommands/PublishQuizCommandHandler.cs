using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands.HandlerCommands
{
    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public PublishQuizCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _uow.Quizzes.GetAll(withNoTracking: false)
                .FirstOrDefaultAsync(q => q.Id == request.QuizId && !q.IsDeleted, cancellationToken);

            if (quiz == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found.");

            if (quiz.Status == QuizStatus.Published)
                return RequestResult<bool>.Success(true);

            quiz.Status = QuizStatus.Published;

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.Unknown, "Failed to publish quiz.");
        }
    }
}
