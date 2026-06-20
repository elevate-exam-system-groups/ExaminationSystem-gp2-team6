using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands.HandlerCommands
{
    public class UpdateQuizCommandHandler : IRequestHandler<UpdateQuizCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateQuizCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(UpdateQuizCommand request, CancellationToken cancellationToken)
        {
            var quiz = await _uow.Quizzes.GetAll(withNoTracking: false)
                .FirstOrDefaultAsync(q => q.Id == request.Id && !q.IsDeleted, cancellationToken);

            if (quiz == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quiz not found.");

            if (!string.IsNullOrEmpty(request.Title) && quiz.Title != request.Title)
            {
                var titleTaken = await _uow.Quizzes.GetAll()
                    .AnyAsync(q => q.Title == request.Title && q.Id != request.Id, cancellationToken);

                if (titleTaken)
                    return RequestResult<bool>.Failure(ErrorCode.AlreadyExists, "A quiz with this title already exists.");
            }

            if (!string.IsNullOrEmpty(request.Title))
                quiz.Title = request.Title;
            if (request.DiplomaId > 0)
                quiz.DiplomaId = request.DiplomaId;
            if (request.DurationMinutes.HasValue)
                quiz.Duration = TimeSpan.FromMinutes(request.DurationMinutes.Value);
            if (request.PassScore.HasValue)
                quiz.PassScore = request.PassScore.Value;
            if (request.MaxAttempts.HasValue)
                quiz.MaxAttempts = request.MaxAttempts.Value;
            if (request.Instructions != null)
                quiz.Instructions = request.Instructions;

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Quiz updated successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to update quiz.");
        }
    }
}
