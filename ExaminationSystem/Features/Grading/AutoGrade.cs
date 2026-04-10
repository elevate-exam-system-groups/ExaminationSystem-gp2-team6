using MediatR;

namespace ExaminationSystem.Features.Grading.AutoGrade;

public record AutoGradeCommand(Guid AttemptId) : IRequest<bool>;

public class AutoGradeCommandHandler : IRequestHandler<AutoGradeCommand, bool>
{
    public Task<bool> Handle(AutoGradeCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
