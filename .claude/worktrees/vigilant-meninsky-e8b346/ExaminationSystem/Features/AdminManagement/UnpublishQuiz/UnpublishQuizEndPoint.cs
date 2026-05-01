using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.UnpublishQuiz.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.UnpublishQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class UnpublishQuizEndPoint : ControllerBase
    {
        [HttpPut("{id}/unpublish")]
        public async Task<IActionResult> UnpublishQuiz(
            [FromRoute] int id,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UnpublishQuizCommand(id), cancellationToken);

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
