using MediatR;

namespace ExaminationSystem.Features.Exams.CreateExam;

public record CreateExamCommand(string Title) : IRequest<Guid>;

public class CreateExamCommandHandler : IRequestHandler<CreateExamCommand, Guid>
{
    public Task<Guid> Handle(CreateExamCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}

public static class CreateExamEndpoint
{
    public static void MapCreateExamEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/exams", async (CreateExamCommand command, IMediator mediator) => 
            Results.Ok(await mediator.Send(command)));
    }
}
