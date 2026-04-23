using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands
{
    public record PublishQuizCommand(int QuizId) : IRequest<RequestResult<bool>>;
}
