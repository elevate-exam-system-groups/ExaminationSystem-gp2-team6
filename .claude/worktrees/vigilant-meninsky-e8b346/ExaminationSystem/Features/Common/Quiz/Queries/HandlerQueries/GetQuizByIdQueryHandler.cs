using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Quiz;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.Common.Quiz.Queries.HandlerQueries
{
    public class GetQuizByIdQueryHandler : IRequestHandler<GetQuizByIdQuery, RequestResult<Domain.Entities.Quiz.Quiz>>
    {
        private readonly IUnitOfWork _uow;

        public GetQuizByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<Domain.Entities.Quiz.Quiz>> Handle(GetQuizByIdQuery request, CancellationToken cancellationToken)
        {
            var quiz = await _uow.Quizzes.GetByIdAsync(request.QuizId);
            if (quiz != null)
                return RequestResult<Domain.Entities.Quiz.Quiz>.Success(quiz);
                
            return RequestResult<Domain.Entities.Quiz.Quiz>.Failure(ErrorCode.NotFound, "Quiz not found");
        }
    }
}
