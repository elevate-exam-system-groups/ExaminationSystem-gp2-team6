using ExaminationSystem.Common.Data;
using ExaminationSystem.Common.Views;
using ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    public class UnpublishQuizEndPoint : ControllerBase
    {
        [HttpPut("{id}/unpublish")]
        public async Task<EndpointResponse<bool>> UnpublishQuiz([FromRoute] int id, [FromServices] IMediator mediator)
        {
            if (id <= 0)
                return EndpointResponse<bool>.Failure(ErrorCode.InvalidData, "Invalid quiz ID.");

            var quizUpdated = await mediator.Send(new UnpublishQuizCommand(id));

            return (quizUpdated.IsSuccess)
                ? EndpointResponse<bool>.Success(quizUpdated.Data, quizUpdated.Message)
                : EndpointResponse<bool>.Failure(quizUpdated.ErrorCode, quizUpdated.Message);
        }
    }
}
