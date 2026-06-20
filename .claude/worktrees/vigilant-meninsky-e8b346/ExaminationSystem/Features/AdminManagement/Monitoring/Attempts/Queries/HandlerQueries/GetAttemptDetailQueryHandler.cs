using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Dtos;
using ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.AdminManagement.Monitoring.Attempts.Queries.HandlerQueries
{
    public class GetAttemptDetailQueryHandler : IRequestHandler<GetAttemptDetailQuery, RequestResult<AttemptDetailDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetAttemptDetailQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<RequestResult<AttemptDetailDto>> Handle(GetAttemptDetailQuery request, CancellationToken cancellationToken)
        {
            var attemptDetail = await _uow.QuizAttempts.GetAll()
                .Where(a => a.Id == request.AttemptId)
                .Select(a => new AttemptDetailDto
                {
                    AttemptId = a.Id,
                    StudentId = a.StudentId,
                    StudentName = a.Student.FullName,
                    QuizTitle = a.Quiz.Title,
                    Score = (int)(a.Score ?? 0),
                    Status = a.Status,
                    SubmittedAt = a.SubmittedAt ?? DateTime.MinValue,
                    QuestionBreakdowns = a.Quiz.Questions.Select(q => new QuestionBreakdownDto
                    {
                        QuestionText = q.QuestionText,
                        StudentAnswer = a.AttemptAnswers
                            .Where(ans => ans.QuestionId == q.Id && ans.SelectedOption != null)
                            .Select(ans => ans.SelectedOption!.Text)
                            .FirstOrDefault(),
                        CorrectAnswer = q.AnswerOptions
                            .Where(opt => opt.IsCorrect)
                            .Select(opt => opt.Text)
                            .FirstOrDefault() ?? "N/A",
                        IsCorrect = a.AttemptAnswers
                            .Any(ans => ans.QuestionId == q.Id && ans.SelectedOption != null && ans.SelectedOption.IsCorrect),
                        PointsEarned = a.AttemptAnswers
                            .Any(ans => ans.QuestionId == q.Id && ans.SelectedOption != null && ans.SelectedOption.IsCorrect) ? 1 : 0
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (attemptDetail == null)
            {
                return RequestResult<AttemptDetailDto>.Failure(ErrorCode.NotFound, "Attempt not found.");
            }

            return RequestResult<AttemptDetailDto>.Success(attemptDetail);
        }
    }
}
