using MediatR;

namespace ExaminationSystem.Features.ExamDelivery.StartAttempt;

public record StartAttemptCommand(Guid ExamId) : IRequest<Guid>;

public class StartAttemptCommandHandler : IRequestHandler<StartAttemptCommand, Guid>
{
    public Task<Guid> Handle(StartAttemptCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
