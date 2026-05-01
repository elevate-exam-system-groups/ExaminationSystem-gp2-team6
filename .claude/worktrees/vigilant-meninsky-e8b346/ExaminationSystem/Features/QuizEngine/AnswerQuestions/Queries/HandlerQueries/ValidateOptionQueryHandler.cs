using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries.HandlerQueries
{
    public class ValidateOptionQueryHandler : IRequestHandler<ValidateOptionQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        public ValidateOptionQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<bool>> Handle(ValidateOptionQuery request, CancellationToken cancellationToken)
        {
            var exiestingOption =  await _uow.AnswerOptions.ExistsAsync(ans=>ans.QuestionId==request.QuestionId&& ans.Id==request.OptionId);
            return exiestingOption 
                ? RequestResult<bool>.Success(true) 
                : RequestResult<bool>.Failure(ErrorCode.NotFound, "Option Not Found");
        }
    }
}
