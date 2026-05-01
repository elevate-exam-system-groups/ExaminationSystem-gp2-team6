using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Commands.HandlerCommand
{
    public class AutoSubmitAttemptCommandHandler:IRequestHandler<AutoSubmitAttemptCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public AutoSubmitAttemptCommandHandler(IUnitOfWork uow) 
        { 
            _uow = uow; 
        }
        public async Task<RequestResult<bool>> Handle(AutoSubmitAttemptCommand request, CancellationToken cancellationToken)
        {
            var attempt =await _uow.QuizAttempts.GetById(request.AttemptId).AsTracking().FirstOrDefaultAsync();
            if (attempt is null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound);

            //Prevent double submit
            if (attempt.Status == AttemptStatus.Submitted)
                return RequestResult<bool>.Failure(ErrorCode.Conflict, "Already submitted");

            attempt.Status = AttemptStatus.Submitted;
            attempt.SubmittedAt = DateTime.UtcNow;
            return await _uow.SaveChangesAsync() > 0
                ? RequestResult<bool>.Success(true)
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to submit attempt");
        }
    }
}
