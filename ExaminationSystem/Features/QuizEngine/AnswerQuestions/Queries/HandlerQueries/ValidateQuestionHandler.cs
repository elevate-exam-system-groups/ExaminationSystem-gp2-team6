using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries.HandlerQueries
{
    public class ValidateQuestionHandler : IRequestHandler<ValidateQuestionQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public ValidateQuestionHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<RequestResult<bool>> Handle(ValidateQuestionQuery request, CancellationToken cancellationToken)
        {
            var exists =await _uow.Questions.ExistsAsync(q => q.Id == request.QuestionId &&q.QuizId==request.QuizId);
            return exists 
                ? RequestResult<bool>.Success(true) 
                : RequestResult<bool>.Failure(ErrorCode.NotFound, "Question not found");
        }
    }
}
