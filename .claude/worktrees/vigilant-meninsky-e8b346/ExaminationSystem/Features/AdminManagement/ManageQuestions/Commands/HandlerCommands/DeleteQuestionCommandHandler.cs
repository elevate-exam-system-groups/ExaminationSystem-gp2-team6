using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands.HandlerCommands
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public DeleteQuestionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            // Single read-only query: existence + quiz status via Select (no Include)
            var info = await _uow.Questions.GetAll()
                .Where(q => q.Id == request.QuestionId && !q.IsDeleted)
                .Select(q => new { QuizStatus = q.Quiz.Status })
                .FirstOrDefaultAsync(cancellationToken);

            if (info == null)
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Question not found.");

            if (info.QuizStatus == QuizStatus.Published)
                return RequestResult<bool>.Failure(ErrorCode.Conflict, "Unpublish quiz first or soft-delete question.");

            // Fetch tracked entity for the mutation
            var question = await _uow.Questions.GetByIdAsync(request.QuestionId);
            _uow.Questions.SoftDelete(question!);

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Question deleted successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to delete question.");
        }
    }
}
