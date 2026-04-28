using ExaminationSystem.Common.Views;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands
{
    public record DeleteQuestionCommand(int QuestionId) : IRequest<RequestResult<bool>>;
}
