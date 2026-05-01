using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries.HandlerQueries
{
    public class GetAttemptForAnsweringQueryHandler : IRequestHandler<GetAttemptForAnsweringQuery, RequestResult<QuizAttemptDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptForAnsweringQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<QuizAttemptDto>> Handle(GetAttemptForAnsweringQuery request, CancellationToken cancellationToken)
        {
            var dto = await _unitOfWork.QuizAttempts.GetAll()
                .Where(a => a.Id == request.attemptId)
                .Select(a => new QuizAttemptDto
                {
                    AttemptId = a.Id,
                    QuizId = a.QuizId,
                    StudentId = a.StudentId,
                    Score = a.Score,
                    Status = a.Status,
                    StartedAt = a.StartedAt,
                    Duration = (int)a.Quiz.Duration.TotalMinutes
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto == null)
                return RequestResult<QuizAttemptDto>.Failure(ErrorCode.NotFound, "Attempt not found");

            return RequestResult<QuizAttemptDto>.Success(dto);
        }
    }
}
