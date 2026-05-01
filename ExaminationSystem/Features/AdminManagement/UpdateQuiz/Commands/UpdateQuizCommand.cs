using ExaminationSystem.Common.Views;
using ExaminationSystem.Domain.Entities.Quiz;
using MediatR;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz.Commands
{
    public record UpdateQuizCommand(int Id, string? Title, int DiplomaId, int? DurationMinutes, int? PassScore, int? MaxAttempts, string? Instructions) : IRequest<RequestResult<bool>>;
}
