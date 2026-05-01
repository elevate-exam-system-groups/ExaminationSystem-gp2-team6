using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Features.QuizEngine.ViewResults.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.ViewResults.Queries.HandlerCommands
{
    public sealed class GetAttemptResultsQueryHandler
    : IRequestHandler<GetAttemptResultsQuery, AttemptResultsDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAttemptResultsQueryHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<AttemptResultsDto> Handle(
            GetAttemptResultsQuery request,
            CancellationToken cancellationToken)
        {
            var projected = await _unitOfWork.QuizAttempts.GetAll()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new
                {
                    a.StudentId,
                    a.Status,
                    a.Score,
                    a.Passed,
                    TotalQuestions = a.AttemptResults.Count(),
                    CorrectCount = a.AttemptResults.Count(r => r.IsCorrect),
                    PerQuestion = a.AttemptResults.Select(r => new QuestionResultDto(
                        r.QuestionId,
                        r.Question.QuestionText,
                        r.StudentAnswerOptionId,
                        r.CorrectAnswerOptionId,
                        r.IsCorrect,
                        r.Question.Explanation
                    )).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException(nameof(QuizAttempt), request.AttemptId);

            if (projected.StudentId != request.RequesterId && !request.RequesterIsAdmin)
                throw new ForbiddenAccessException();

            if (projected.Status == AttemptStatus.InProgress)
                throw new ForbiddenAccessException(
                    "Results are not available while the attempt is still in progress.");

            return new AttemptResultsDto(
                Score: projected.Score!.Value,
                Passed: projected.Passed!.Value,
                TotalQuestions: projected.TotalQuestions,
                CorrectCount: projected.CorrectCount,
                PerQuestion: projected.PerQuestion
            );
        }
    }
}
