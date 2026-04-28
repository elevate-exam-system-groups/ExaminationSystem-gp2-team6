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
            var attempt = await _unitOfWork.QuizAttempts.GetAll()
                .AsNoTracking()
                .Include(a => a.AttemptResults)
                    .ThenInclude(r => r.Question)
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken)
                ?? throw new NotFoundException(nameof(QuizAttempt), request.AttemptId);

            bool isOwner = attempt.StudentId == request.RequesterId;

            if (!isOwner && !request.RequesterIsAdmin)
                throw new ForbiddenAccessException();

            if (attempt.Status == AttemptStatus.InProgress)
                throw new ForbiddenAccessException(
                    "Results are not available while the attempt is still in progress.");

            var results = attempt.AttemptResults;

            var perQuestion = results.Select(r => new QuestionResultDto(
                QuestionId: r.QuestionId,
                QuestionText: r.Question.QuestionText,
                StudentAnswerOptionId: r.StudentAnswerOptionId,
                CorrectAnswerOptionId: r.CorrectAnswerOptionId,
                IsCorrect: r.IsCorrect,
                Explanation: r.Question.Explanation
            )).ToList();

            return new AttemptResultsDto(
                Score: attempt.Score!.Value,
                Passed: attempt.Passed!.Value,
                TotalQuestions: results.Count,
                CorrectCount: results.Count(r => r.IsCorrect),
                PerQuestion: perQuestion
            );
        }
    }
}
