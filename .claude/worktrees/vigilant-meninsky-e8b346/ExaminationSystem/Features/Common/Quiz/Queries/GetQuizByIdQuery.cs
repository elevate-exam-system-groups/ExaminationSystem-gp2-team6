using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using MediatR;

namespace ExaminationSystem.Features.Common.Quiz.Queries
{
    public record GetQuizByIdQuery(int QuizId) : IRequest<RequestResult<Domain.Entities.Quiz.Quiz>>;
}
