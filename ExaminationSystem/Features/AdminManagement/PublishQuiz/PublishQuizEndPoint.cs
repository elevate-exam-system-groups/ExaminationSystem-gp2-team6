using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class PublishQuizEndPoint: ControllerBase
    {
        [HttpPut("{id}/publish")]
        public async Task<EndpointResponse<bool>> PublishQuiz([FromRoute] int id, [FromServices] IMediator mediator)
        {
            if (id <= 0)
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, "Invalid quiz ID.");
            
            var quizUpdated = await mediator.Send(new PublishQuizCommand(id));

            return (quizUpdated.IsSuccess)
                ? EndpointResponse<bool>.Success(quizUpdated.Data, quizUpdated.Message)
                : EndpointResponse<bool>.Failure(quizUpdated.ErrorCode, quizUpdated.Message);
        }
    }
}
