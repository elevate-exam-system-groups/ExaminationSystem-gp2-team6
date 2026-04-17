using ExaminationSystem.Common.Exceptions;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Domain.Entities.Shared.Enums.AttemptStatus;
using ExaminationSystem.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.QuizEngine.SubmitQuiz.Commands.HandlersCommands
{
    public sealed class SubmitQuizCommandHandler : IRequestHandler<SubmitQuizCommand, SubmitQuizResult>
    {
        private readonly IUnitOfWork _uow;

        public SubmitQuizCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<SubmitQuizResult> Handle(SubmitQuizCommand request,CancellationToken cancellationToken)
        {
            var attempt = await _uow.QuizAttempts
                .Include(a => a.Quiz)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(q => q.AnswerOptions)
                .Include(a => a.AttemptAnswers)
                .FirstOrDefaultAsync(a => a.Id == request.AttemptId, cancellationToken)
                ?? throw new NotFoundException(nameof(QuizAttempt), request.AttemptId);

            if (attempt.StudentId != request.StudentId)
                throw new ForbiddenAccessException();

            if (attempt.Status is AttemptStatus.Submitted or AttemptStatus.TimedOut)
            {
                throw new ConflictException(
                    $"Attempt {request.AttemptId} was already submitted.",
                    new SubmitQuizResult(attempt.Id, attempt.Score!.Value, attempt.Passed!.Value));
            }

            bool isTimedOut = attempt.IsTimedOut(attempt.Quiz.Duration);
            attempt.Status = isTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;
            attempt.SubmittedAt = DateTime.UtcNow;

            var questions = attempt.Quiz.Questions.ToList();
            int totalQuestions = questions.Count;

            var correctOptions = questions
                .ToDictionary(
                    q => q.Id,
                    q => q.AnswerOptions.FirstOrDefault(o => o.IsCorrect)?.Id);

            var studentAnswers = attempt.AttemptAnswers
                .ToDictionary(a => a.QuestionId, a => a.SelectedOptionId);

            int correctCount = 0;
            var results = new List<Domain.Entities.AttemptResult.AttemptResult>();

            foreach (var question in questions)
            {
                studentAnswers.TryGetValue(question.Id, out int? studentOption);
                correctOptions.TryGetValue(question.Id, out int? correctOption);

                bool isCorrect = studentOption.HasValue
                                 && studentOption == correctOption;

                if (isCorrect) correctCount++;

                results.Add(new Domain.Entities.AttemptResult.AttemptResult
                {
                    AttemptId = attempt.Id,
                    QuestionId = question.Id,
                    StudentAnswerOptionId = studentOption,
                    CorrectAnswerOptionId = correctOption,
                    IsCorrect = isCorrect
                });
            }

            double score = totalQuestions > 0
                ? Math.Round((double)correctCount / totalQuestions * 100, 2)
                : 0;

            bool passed = score >= attempt.Quiz.PassScore;

            attempt.Score = score;
            attempt.Passed = passed;

            await _context.AttemptResults.AddRangeAsync(results, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new SubmitQuizResult(attempt.Id, score, passed);
        }
    }
}
