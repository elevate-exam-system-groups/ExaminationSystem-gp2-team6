using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Dtos;
using ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Analytics.Queries.HandlerQueries
{
    public sealed class GetPerformanceAnalyticsQueryHandler
        : IRequestHandler<GetPerformanceAnalyticsQuery, RequestResult<AnalyticsDashboardDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPerformanceAnalyticsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RequestResult<AnalyticsDashboardDto>> Handle(GetPerformanceAnalyticsQuery request, CancellationToken cancellationToken)
        {
            var attemptsQuery = _unitOfWork.QuizAttempts.GetAll().AsNoTracking();
            var answersQuery = _unitOfWork.AttemptAnswers.GetAll().AsNoTracking();

            if (request.From.HasValue)
            {
                attemptsQuery = attemptsQuery.Where(a => a.SubmittedAt >= request.From.Value);
                answersQuery = answersQuery.Where(a => a.AnsweredAt >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                attemptsQuery = attemptsQuery.Where(a => a.SubmittedAt <= request.To.Value);
                answersQuery = answersQuery.Where(a => a.AnsweredAt <= request.To.Value);
            }

            if (request.DiplomaId.HasValue)
            {
                attemptsQuery = attemptsQuery.Where(a => a.Quiz.DiplomaId == request.DiplomaId.Value);
                answersQuery = answersQuery.Where(a => a.Question.Quiz.DiplomaId == request.DiplomaId.Value);
            }

            attemptsQuery = attemptsQuery.Where(a => a.SubmittedAt != null);

            var passRateByQuizTask = attemptsQuery
                .GroupBy(a => new { a.QuizId, a.Quiz.Title })
                .Select(g => new QuizPassRateDto(
                    g.Key.QuizId,
                    g.Key.Title,
                    g.Count() == 0 ? 0 : (double)g.Count(x => x.Passed == true) / g.Count()
                ))
                .ToListAsync(cancellationToken);

            var avgScoreByDiplomaTask = attemptsQuery
                .GroupBy(a => new { a.Quiz.DiplomaId, a.Quiz.Diploma.Title })
                .Select(g => new DiplomaAvgScoreDto(
                    g.Key.DiplomaId,
                    g.Key.Title,
                    g.Count() == 0 ? 0 : g.Average(x => x.Score ?? 0)
                ))
                .ToListAsync(cancellationToken);

            var attemptsOverTimeTask = attemptsQuery
                .GroupBy(a => a.SubmittedAt!.Value.Date)
                .Select(g => new AttemptsOverTimeDto(
                    g.Key.ToString("yyyy-MM-dd"),
                    g.Count()
                ))
                .OrderBy(dto => dto.Date)
                .ToListAsync(cancellationToken);

            var topFailedQuestionsTask = answersQuery
                .GroupBy(a => new { a.QuestionId, a.Question.QuestionText })
                .Select(g => new TopFailedQuestionDto(
                    g.Key.QuestionId,
                    g.Key.QuestionText,
                    (double)g.Count(x => x.SelectedOption!.IsCorrect) / g.Count()
                ))
                .Where(dto => dto.CorrectRate < 0.40)
                .OrderBy(dto => dto.CorrectRate)
                .Take(10)
                .ToListAsync(cancellationToken);

            await Task.WhenAll(passRateByQuizTask, avgScoreByDiplomaTask, attemptsOverTimeTask, topFailedQuestionsTask);

            var dashboard = new AnalyticsDashboardDto
            {
                PassRateByQuiz = await passRateByQuizTask,
                AvgScoreByDiploma = await avgScoreByDiplomaTask,
                AttemptsOverTime = await attemptsOverTimeTask,
                TopFailedQuestions = await topFailedQuestionsTask
            };

            return RequestResult<AnalyticsDashboardDto>.Success(dashboard);
        }
    }
}
