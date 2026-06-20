using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Features.AdminManagement.CreateQuiz.Commands;
using ExaminationSystem.Features.Common.Diploma.Queries;
using ExaminationSystem.Features.Common.Quiz.Queries;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.CreateQuiz.Orchestrator.HandlerOrchestrator
{
    public class CreateQuizOrchestratorHandler : IRequestHandler<CreateQuizOrchestrator, RequestResult<bool>>
    {
        private readonly IMediator _mediator;

        public CreateQuizOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResult<bool>> Handle(CreateQuizOrchestrator request, CancellationToken cancellationToken)
        {
            // Check if Diploma exists
            var diplomaExists = await _mediator.Send(new IsDiplomaIdExistQuery(request.DiplomaId), cancellationToken);
            if (!diplomaExists.IsSuccess)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Diploma not found");

            // check if quiz title already exists
            var titleExists = await _mediator.Send(new IsQuizTitleExistQuery(request.Title), cancellationToken);
            if (titleExists.IsSuccess)
            {
                return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, "Quiz with this title already exists.");
            }

            // create quiz
            return await _mediator.Send(new CreateQuizCommand( request.Title, request.DiplomaId, request.DurationMinutes,request.PassScore,request.MaxAttempts,request.Instructions), cancellationToken);
        }
    }
}
