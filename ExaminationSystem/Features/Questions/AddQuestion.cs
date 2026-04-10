using MediatR;

namespace ExaminationSystem.Features.Questions.AddQuestion;

public record AddQuestionCommand(string QuestionText) : IRequest<Guid>;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    public Task<Guid> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}
