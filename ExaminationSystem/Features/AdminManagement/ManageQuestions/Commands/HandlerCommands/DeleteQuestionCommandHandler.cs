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
            var question = await _uow.Questions.GetById(request.QuestionId, q => q.Quiz)
                .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Question not found.");
            }

            if (question.Quiz.Status == QuizStatus.Published)
            {
                return RequestResult<bool>.Failure(ErrorCode.Conflict, "Unpublish quiz first or soft-delete question.");
            }

            _uow.Questions.SoftDelete(request.QuestionId);

            return await _uow.SaveChangesAsync(cancellationToken) > 0
                ? RequestResult<bool>.Success(true, "Question deleted successfully.")
                : RequestResult<bool>.Failure(ErrorCode.DatabaseError, "Failed to delete question.");
        }
    }
}
