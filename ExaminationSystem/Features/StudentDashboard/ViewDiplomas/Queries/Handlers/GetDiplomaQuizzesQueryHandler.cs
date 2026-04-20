using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Contracts;
using ExaminationSystem.Domain.Entities.Shared.Enums.Diploma;
using ExaminationSystem.Domain.Entities.Shared.Enums.Quiz;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas;
using ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Features.StudentDashboard.ViewDiplomas.Queries.Handlers;

public sealed class GetDiplomaQuizzesQueryHandler
    : IRequestHandler<GetDiplomaQuizzesQuery, RequestResult<IReadOnlyList<DiplomaQuizItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDiplomaQuizzesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RequestResult<IReadOnlyList<DiplomaQuizItemDto>>> Handle(
        GetDiplomaQuizzesQuery request,
        CancellationToken cancellationToken)
    {
        var diplomaExistsAndPublished = await _unitOfWork.Diplomas.GetAll()
            .AsNoTracking()
            .AnyAsync(
                d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published,
                cancellationToken);

        if (!diplomaExistsAndPublished)
        {
            return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Failure(
                ErrorCode.NotFound,
                "Diploma not found.");
        }

        var isStudentEnrolled = await _unitOfWork.StudentDiplomaEnrollments.GetAll()
            .AsNoTracking()
            .AnyAsync(
                e => e.DiplomaId == request.DiplomaId && e.StudentId == request.StudentId,
                cancellationToken);

        if (!isStudentEnrolled)
        {
            return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Failure(
                ErrorCode.Unauthorized,
                "Student is not enrolled in this diploma.");
        }

        var quizzes = await _unitOfWork.Quizzes.GetAll()
            .AsNoTracking()
            .Where(q => q.DiplomaId == request.DiplomaId && q.Status == QuizStatus.Published)
            .OrderBy(q => q.Id)
            .Select(q => new DiplomaQuizItemDto
            {
                Id = q.Id,
                Title = q.Title,
                DurationMinutes = (int)q.Duration.TotalMinutes,
                AttemptCount = q.QuizAttempts.Count(a => a.StudentId == request.StudentId),
                LastScore = q.QuizAttempts
                    .Where(a => a.StudentId == request.StudentId)
                    .OrderByDescending(a => a.SubmittedAt)
                    .Select(a => (int?)a.Score)
                    .FirstOrDefault(),
                Status = q.Status.ToString().ToLowerInvariant()
            })
            .ToListAsync(cancellationToken);

        return RequestResult<IReadOnlyList<DiplomaQuizItemDto>>.Success(quizzes);
    }
}
