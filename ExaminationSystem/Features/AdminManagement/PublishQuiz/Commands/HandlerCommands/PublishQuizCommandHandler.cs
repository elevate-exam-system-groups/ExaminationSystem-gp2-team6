using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.Common.Quiz.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands.HandlerCommands
{
    public class PublishQuizCommandHandler : IRequestHandler<PublishQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public PublishQuizCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(PublishQuizCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetQuizByIdQuery(request.QuizId), cancellationToken);
            if (!result.IsSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found");
            }

            var quiz = result.Data;
            if (quiz.Status == QuizStatus.Published)
            {
                // Already unpublished/draft
                return RequestResult<bool>.Success(true);
            }

            quiz.Status = QuizStatus.Published;
            _uow.Quizzes.Update(quiz);

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.Unknown, "Failed to publish quiz");
        }
    }
}
