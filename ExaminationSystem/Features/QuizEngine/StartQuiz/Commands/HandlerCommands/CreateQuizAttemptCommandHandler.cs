using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Commands.HandlerCommands
{
    public class CreateQuizAttemptCommandHandler : IRequestHandler<CreateQuizAttemptCommand, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _uow;
        public CreateQuizAttemptCommandHandler(IMediator mediator, IUnitOfWork uow)
        {
            _mediator = mediator;
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
        {
            _uow.QuizAttempts.Add(new QuizAttempt
            {
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                Status = AttemptStatus.InProgress,
                StartedAt = DateTime.UtcNow
                
            });
            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to create quiz attempt.");
        }
    }
}
