using MediatR;

namespace ExaminationSystem.Features.Exams.GetExamById;

public record GetExamByIdQuery(Guid Id) : IRequest<ExamDto>;
public record ExamDto(Guid Id, string Title);

public class GetExamByIdQueryHandler : IRequestHandler<GetExamByIdQuery, ExamDto>
{
    public Task<ExamDto> Handle(GetExamByIdQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new ExamDto(request.Id, "Sample Title"));
    }
}

public static class GetExamByIdEndpoint
{
    public static void MapGetExamByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/exams/{id:guid}", async (Guid id, IMediator mediator) => 
            Results.Ok(await mediator.Send(new GetExamByIdQuery(id))));
    }
}
