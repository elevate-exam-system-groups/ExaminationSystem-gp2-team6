using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.Common.Quiz.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands.HandlerCommands
{
    public class UnpublishQuizCommandHandler : IRequestHandler<UnpublishQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;

        public UnpublishQuizCommandHandler(IUnitOfWork uow, IMediator mediator)
        {
            _uow = uow;
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(UnpublishQuizCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetQuizByIdQuery(request.QuizId), cancellationToken);
            if (!result.IsSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found");
            }

            var quiz = result.Data;
            if (quiz.Status == QuizStatus.Draft)
            {
                // Already unpublished/draft
                return RequestResult<bool>.Success(true);
            }
            
            quiz.Status = QuizStatus.Draft;
            _uow.Quizzes.Update(quiz);
  
            return await _uow.SaveChangesAsync(cancellationToken)>0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.Unknown, "Failed to unpublish quiz");
        }
    }
}
