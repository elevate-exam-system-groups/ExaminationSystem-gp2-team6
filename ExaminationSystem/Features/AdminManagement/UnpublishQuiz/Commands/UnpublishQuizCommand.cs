using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands
{
    public record UnpublishQuizCommand(int QuizId) : IRequest<RequestResult<bool>>;
}
