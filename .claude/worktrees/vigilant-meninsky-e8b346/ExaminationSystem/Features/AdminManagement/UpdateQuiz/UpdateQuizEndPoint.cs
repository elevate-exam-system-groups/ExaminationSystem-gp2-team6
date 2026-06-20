using ExaminationSystem.Common.Data;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.Orchestrator;
using ExaminationSystem.Features.AdminManagement.UpdateQuiz.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Features.AdminManagement.UpdateQuiz
{
    [ApiController]
    [Route("api/admin/quizzes")]
    [Authorize(Roles = "Admin")]
    public class UpdateQuizEndPoint : ControllerBase
    {
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuiz(
            [FromRoute] int id,
            [FromBody] UpdateQuizRequestViewModel request,
            [FromServices] IMediator mediator,
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(
                new UpdateQuizOrchestrator(
                    id, request.Title, request.DiplomaId, request.DurationMinutes,
                    request.PassScore, request.MaxAttempts, request.Instructions),
                cancellationToken);

            if (!result.IsSuccess)
                return result.ErrorCode switch
                {
                    ErrorCode.NotFound => NotFound(new { result.Message }),
                    ErrorCode.AlreadyExists => Conflict(new { result.Message }),
                    _ => BadRequest(new { result.Message })
                };

            return Ok(new { result.Message });
        }
    }
}
