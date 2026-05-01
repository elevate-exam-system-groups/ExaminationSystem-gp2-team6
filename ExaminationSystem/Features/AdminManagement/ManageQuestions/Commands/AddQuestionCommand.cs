using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.ManageQuestions.Dtos;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.ManageQuestions.Commands
{
    public record AddQuestionCommand(int QuizId, string Text, List<OptionDto> Options, string? Explanation) : IRequest<RequestResult<int>>;
}
