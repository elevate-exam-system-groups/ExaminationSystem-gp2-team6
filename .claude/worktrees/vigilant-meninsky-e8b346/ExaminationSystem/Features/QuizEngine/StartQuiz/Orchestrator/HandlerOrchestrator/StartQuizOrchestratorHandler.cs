using AutoMapper;
using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Commands;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Orchestrator.HandlerOrchestrator
{
    public class StartQuizOrchestratorHandler : IRequestHandler<StartQuizOrchestrator, RequestResult<IEnumerable<QuestionDto>>>
    {
        private readonly IMediator _mediator;
        public StartQuizOrchestratorHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
        }
        public async Task<RequestResult<IEnumerable<QuestionDto>>> Handle(StartQuizOrchestrator request, CancellationToken cancellationToken)
        {
            //check if quiz exists
            var quiz = await _mediator.Send(new IsQuizIdExistQuery(request.QuizId), cancellationToken);
            if (quiz is null)
                return RequestResult<IEnumerable<QuestionDto>>.Failure(ErrorCode.NotFound, "Quiz not found.");

            //check inProgress attempt
            var attemptLimitCheck = await _mediator.Send(new CheckInProgressAttemptQuery(request.QuizId, request.StudentId), cancellationToken);
            if (!attemptLimitCheck.IsSuccess)
                return RequestResult<IEnumerable<QuestionDto>>.Failure(ErrorCode.AttemptLimitReached, attemptLimitCheck.Message);

            //create quiz attempt
            var createAttemptResult = await _mediator.Send(new CreateQuizAttemptCommand(request.QuizId, request.StudentId), cancellationToken);
            if (!createAttemptResult.IsSuccess)
                return RequestResult<IEnumerable<QuestionDto>>.Failure(createAttemptResult.ErrorCode, createAttemptResult.Message);

            //return quiz questions
            var questionsResult = await _mediator.Send(new GetQuestionsByQuizIdQuery(request.QuizId), cancellationToken);

            return questionsResult;
        }
    }
}
