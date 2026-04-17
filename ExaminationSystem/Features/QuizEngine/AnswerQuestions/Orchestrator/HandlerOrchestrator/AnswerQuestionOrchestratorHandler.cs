using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Question;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries;
using ExaminationSystem.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Orchestrator.HandlerOrchestrator
{
    public class AnswerQuestionOrchestratorHandler : IRequestHandler<AnswerQuestionOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;
        public AnswerQuestionOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _uow = unitOfWork;
        }

        public async Task<RequestResult<bool>> Handle(AnswerQuestionOrchestrator request, CancellationToken cancellationToken)
        {
            // 1. Get Attempt
            var attemptResult = await _mediator.Send(new GetAttemptForAnsweringQuery(request.AttemptId));
            if (!attemptResult.IsSuccess || attemptResult.Data is null)
                return RequestResult<bool>.Failure(attemptResult.ErrorCode, attemptResult.Message);
            
            var attempt = attemptResult.Data;
            if (attempt.StudentId != request.StudentId)
                return RequestResult<bool>.Failure(ErrorCode.Forbidden);

            // Status
            if (attempt.Status != AttemptStatus.InProgress)
                return RequestResult<bool>.Failure(ErrorCode.Conflict);

            // Timer
            var isTimedOut = DateTime.UtcNow > attempt.StartedAt.AddMinutes(attempt.Duration);
            if (isTimedOut)
            {
                await _mediator.Send(new AutoSubmitAttemptCommand(attempt.AttemptId));
                return RequestResult<bool>.Failure(ErrorCode.Gone, "Attempt expired");
            }

            // 5. Validate Question
            var questionValid = await _mediator.Send(new ValidateQuestionQuery(attempt.QuizId, request.QuestionId));
            if (!questionValid.IsSuccess)
                return RequestResult<bool>.Failure(questionValid.ErrorCode, questionValid.Message);

            // 6. Validate Option
            var optionValid = await _mediator.Send(new ValidateOptionQuery(request.QuestionId, request.OptionId));
            if (!optionValid.IsSuccess)
                return RequestResult<bool>.Failure(optionValid.ErrorCode, optionValid.Message);

            // 7. Upsert Answer
            await _mediator.Send(new AnswerQuestionCommand(attempt.AttemptId,request.QuestionId,request.OptionId));
            
            return RequestResult<bool>.Success(true);
        }
    }
}
