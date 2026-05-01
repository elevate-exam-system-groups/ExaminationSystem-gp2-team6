using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.PublishQuiz.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.PublishQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class PublishQuizEndPoint : ControllerBase
    {
        [HttpPut("{id}/publish")]
        public async Task<IActionResult> PublishQuiz(
            [FromRoute] int id,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new PublishQuizCommand(id), cancellationToken);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.InvalidData => BadRequest(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };

            return Ok(new { result.Message });
        }
    }
}
