using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Infrastructure.Persistence.DB.Context;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.StartQuiz.Commands.HandlerCommands
{
    public class CreateQuizAttemptCommandHandler : IRequestHandler<CreateQuizAttemptCommand, RequestResult<bool>>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _db;
        public CreateQuizAttemptCommandHandler(IMediator mediator, AppDbContext db)
        {
            _mediator = mediator;
            _db = db;
        }

        public async Task<RequestResult<bool>> Handle(CreateQuizAttemptCommand request, CancellationToken cancellationToken)
        {
            _db.QuizAttempts.Add(new QuizAttempt
            {
                QuizId = request.QuizId,
                StudentId = request.StudentId,
                Status = AttemptStatus.InProgress,
                StartedAt = DateTime.UtcNow
                
            });
            return await _db.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to create quiz attempt.");
        }
    }
}
