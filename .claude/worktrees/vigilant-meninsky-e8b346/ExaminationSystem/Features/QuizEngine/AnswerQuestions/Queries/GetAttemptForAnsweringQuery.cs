using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.QuizAttempt;
using ExaminationSystem.Features.QuizEngine.AnswerQuestions.Dtos;
using MediatR;

namespace ExaminationSystem.Features.QuizEngine.AnswerQuestions.Queries
{
    public record GetAttemptForAnsweringQuery(int attemptId) : IRequest<RequestResult<QuizAttemptDto>>;
}
