using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands;
using ExaminationSystem.Features.Common.Diploma.Queries;
using ExaminationSystem.Features.Common.Quiz.Queries;
using ExaminationSystem.Features.QuizEngine.StartQuiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator.HandlerOrchestrator
{
    public class UpdateQuizOrchestratorHandler : IRequestHandler<UpdateQuizOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;

        public UpdateQuizOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(UpdateQuizOrchestrator request, CancellationToken cancellationToken)
        {   
            var quizResult = await _mediator.Send(new IsQuizIdExistQuery(request.Id), cancellationToken);
            if (quizResult != null&&!quizResult.IsSuccess) 
                return RequestResult<bool>.Failure(quizResult.ErrorCode, quizResult.Message);

            var diplomaExists = await _mediator.Send(new IsDiplomaIdExistQuery(request.DiplomaId), cancellationToken);
            if (!diplomaExists.IsSuccess)            
                return RequestResult<bool>.Failure(diplomaExists.ErrorCode, diplomaExists.Message);
          
            return await _mediator.Send(new UpdateQuizCommand(request.Id, request.Title, request.DiplomaId, request.DurationMinutes, request.PassScore, request.MaxAttempts, request.Instructions), cancellationToken);  
        }
    }
}
