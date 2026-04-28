using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Dtos;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands
{
    public record UpdateQuestionCommand(int QuestionId, string Text, List<OptionDto> Options, string? Explanation) : IRequest<RequestResult<bool>>;
}
